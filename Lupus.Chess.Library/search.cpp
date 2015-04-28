#include "search.h"
#include "board.h"
#include "executor.h"

namespace chess {
    int quiescence_search(std::shared_ptr<executor> exe, int alpha, int beta, int depth) {
        exe->update();
        auto stand_pat = exe->evaluate();

        if (exe->terminal())
            return stand_pat;
        if (depth == 0)
            return stand_pat;
        if (stand_pat >= beta)
            return beta;
        if (alpha < stand_pat)
            alpha = stand_pat;

        for (auto&& move : exe->allowed_captures()) {
            exe->make_move(move);
            auto score = -quiescence_search(exe, -beta, -alpha, depth - 1);
            exe->undo_move();

            if (score >= beta)
                return beta;
            if (score > alpha)
                alpha = score;
        }

        return alpha;
    }

    int alpha_beta_search(std::shared_ptr<executor> exe, int alpha, int beta, int depth, int quiescene_depth) {
        exe->update();

        if (exe->terminal())
            return exe->evaluate();
        if (depth == 0)
            return quiescence_search(exe, alpha, beta, quiescene_depth);

        auto best_score = std::numeric_limits<int>::min();

        for (auto&& move : exe->allowed_moves()) {
            exe->make_move(move);
            auto score = -alpha_beta_search(exe, -beta, -alpha, depth - 1, quiescene_depth);
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
}
