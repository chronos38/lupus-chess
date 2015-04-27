#include "executor.h"
#include "board.h"
#include "piece.h"
#include "move.h"
#include <future>
#include <algorithm>
#include <unordered_map>

namespace chess {
    executor::executor(executor&& other) {
        std::swap(stack_, other.stack_);
        swap(board_, other.board_);
        swap(pieces_, other.pieces_);
    }

    executor::executor(const executor& other) {
        stack_ = other.stack_;
        board_ = other.board_->clone();
        pieces_ = std::deque<std::shared_ptr<piece>>(begin(other.pieces_), end(other.pieces_));
        moves_ = std::vector<std::shared_ptr<move>>(begin(other.moves_), end(other.moves_));
    }

    executor::executor(std::shared_ptr<board> board) : board_(board) {
        moves_.reserve(128);
        for (auto rank = 1; rank != 9; rank++)
            for (auto file = 'a'; file != 'i'; file++)
                if (board->get(file, rank))
                    pieces_.emplace_back(std::make_shared<piece>(board, (std::string(1, file) + static_cast<char>(rank + '0')).c_str()));
    }

    executor executor::clone() const {
        return std::move(executor(*this));
    }

    void executor::update() {
        std::vector<std::future<std::shared_ptr<piece>>> tasks;
        tasks.reserve(pieces_.size());
        moves_.clear();

        for_each(begin(pieces_), end(pieces_), [&] (std::shared_ptr<piece> p) {
            tasks.emplace_back(async(std::launch::async, [] (std::shared_ptr<piece> piece) {
                piece->update();
                return piece;
            }, p));
        });

        for (auto&& task : tasks) {
            auto piece = task.get();
            if (piece->color() != board_->active_color())
                continue;
            auto moves = piece->allowed_moves();
            moves_.insert(end(moves_), begin(moves), end(moves));
        }
    }

    std::vector<std::shared_ptr<move>> executor::allowed_moves() const {
        return moves_;
    }

    int executor::evaluate() const {
        std::unordered_map<piece_color, int> material = {
                { white, 0 },
                { black, 0 }
            }, attack = {
                { white, 0 },
                { black, 0 }
            }, defense = {
                { white, 0 },
                { black, 0 }
            }, position = {
                { white, 0 },
                { black, 0 }
            };

        for (auto&& piece : pieces_) {
            material[piece->color()] += piece->score();
            attack[piece->color()] += piece->attack_score();
            defense[piece->color()] += piece->defense_score();
            position[piece->color()] += piece->position_score();
        }

        return material[black] - material[white] +
            attack[black] - attack[white] +
            defense[black] - defense[white] +
            position[black] - position[white];
    }

    void executor::make_move(std::shared_ptr<move> move) {
        auto square = board_->get(move->to());
        if (square) {
            for (auto it = begin(pieces_), end = std::end(pieces_); it != end; ++it) {
                auto piece = *it;
                if (strcmp(move->to(), piece->position()) == 0 && static_cast<uint8_t>(piece->value()) == square) {
                    pieces_.erase(it);
                    erased_.push(piece);
                    break;
                }
            }
        }

        move->execute();
        stack_.push(move);
        board_->increment_halfmove();
        board_->toggle_active_color();
    }

    void executor::undo_move() {
        if (erased_.size()) {
            pieces_.push_back(std::move(erased_.top()));
            erased_.pop();
        }

        stack_.undo();
        board_->decrement_halfmove();
        board_->toggle_active_color();
    }

    executor& executor::operator=(executor&& other) {
        std::swap(stack_, other.stack_);
        swap(board_, other.board_);
        swap(pieces_, other.pieces_);

        other.stack_.clear();
        other.board_ = nullptr;
        other.pieces_.clear();

        return *this;
    }

    executor& executor::operator=(const executor& other) {
        stack_ = other.stack_;
        board_ = other.board_->clone();
        pieces_ = std::deque<std::shared_ptr<piece>>(begin(other.pieces_), end(other.pieces_));
        moves_ = std::vector<std::shared_ptr<move>>(begin(other.moves_), end(other.moves_));
        return *this;
    }
}
