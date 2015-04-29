#include "search.h"
#include "executor.h"
#include "move.h"
#include <future>
#include <algorithm>

namespace chess {
    int quiescence_search(std::shared_ptr<executor> exe, int alpha, int beta, int depth) {
        auto stand_pat = exe->evaluate();

        if (depth == 0)
            return stand_pat;
        exe->update();

        if (exe->terminal())
            return stand_pat;
        if (stand_pat >= beta)
            return beta;
        if (alpha < stand_pat)
            alpha = stand_pat;
        exe->sort_captures();

        for (auto&& move : exe->allowed_captures()) {
            exe->make_move(move);
            auto score = -quiescence_search(exe->clone(), -beta, -alpha, depth - 1);
            exe->undo_move();

            if (score >= beta)
                return beta;
            if (score > alpha)
                alpha = score;
        }

        return alpha;
    }

    int alpha_beta_search(std::shared_ptr<executor> exe, int alpha, int beta, int depth) {
        if (depth == 0)
            return -quiescence_search(exe, -beta, -alpha, quiescence_search_depth);
        exe->update();

        if (exe->terminal())
            return exe->evaluate();
        auto best_score = std::numeric_limits<int>::min();
        exe->sort_moves();

        for (auto&& move : exe->allowed_moves()) {
            exe->make_move(move);
            auto score = -alpha_beta_search(exe->clone(), -beta, -alpha, depth - 1);
            exe->undo_move();

            if (score >= beta)
                return score;
            if (score <= best_score)
                continue;
            best_score = score;

            if (score <= alpha)
                continue;
            alpha = score;
        }

        return best_score;
    }

    std::shared_ptr<move> iterative_search(std::shared_ptr<executor> exe, int depth) {
        exe->update();
        auto moves = exe->allowed_moves();
        std::vector<std::future<int>> tasks;
        std::vector<std::pair<int, std::shared_ptr<move>>> scores;
        tasks.reserve(moves.size() - 1);
        scores.reserve(moves.size());

        for (int i = moves.size() - 2; i >= 0; i--) {
            tasks.emplace_back(async([] (std::shared_ptr<executor> e, std::shared_ptr<move> m, int d) {
                auto clone = e->clone();
                m->set_board(clone->board());
                clone->make_move(m);
                return alpha_beta_search(clone, initial_alpha, initial_beta, d);
            }, exe, moves[i], depth));
        }

        exe->make_move(moves.back());
        auto score = alpha_beta_search(exe->clone(), initial_alpha, initial_beta, depth);
        exe->undo_move();
        scores.push_back(make_pair(score, moves.back()));

        for (int i = moves.size() - 2; i >= 0; i--) {
            scores.push_back(make_pair(tasks[i].get(), moves[i]));
        }

        sort(begin(scores), end(scores), [] (const std::pair<int, std::shared_ptr<move>>& lhs, const std::pair<int, std::shared_ptr<move>>& rhs) {
            return lhs > rhs;
        });

        return scores.front().second;
    }
}
