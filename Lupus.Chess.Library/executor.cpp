﻿#include "executor.h"
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
        swap(moves_, other.moves_);
        swap(captures_, other.captures_);
    }

    executor::executor(const executor& other) {
        stack_ = other.stack_;
        board_ = other.board_->clone();
        for (auto rank = 1; rank != 9; rank++)
            for (auto file = 'a'; file != 'i'; file++)
                if (board_->get(file, rank))
                    pieces_.emplace_back(std::make_shared<piece>(board_, (std::string(1, file) + static_cast<char>(rank + '0')).c_str()));
    }

    executor::executor(std::shared_ptr<chess::board> board) : board_(board) {
        moves_.reserve(128);
        for (auto rank = 1; rank != 9; rank++)
            for (auto file = 'a'; file != 'i'; file++)
                if (board->get(file, rank))
                    pieces_.emplace_back(std::make_shared<piece>(board, (std::string(1, file) + static_cast<char>(rank + '0')).c_str()));
    }

    std::shared_ptr<executor> executor::clone() const {
        return std::make_shared<executor>(*this);
    }

    void executor::update() {
        // Prepare update
        moves_.clear();
        captures_.clear();
        terminal_ = count_if(std::begin(*board_), std::end(*board_), [] (uint8_t square) {
            return square ? value_to_type(static_cast<piece_value>(square)) == king : false;
        }) != 2;
        if (terminal_)
            return;

        for (auto&& piece : pieces_) {
            piece->update();
            if (piece->color() != board_->active_color())
                continue;
            auto moves = piece->allowed_moves();
            moves_.insert(end(moves_), begin(moves), end(moves));
            copy_if(begin(moves), end(moves), back_inserter(captures_), [&] (std::shared_ptr<move> move) {
                return board_->get(move->to()) != 0;
            });
        }
    }

    void executor::sort_moves() {
        // Move sorting
        auto color = board_->active_color();
        std::vector<int> scores;
        std::vector<std::shared_ptr<move>> sorted;
        sorted.reserve(moves_.size());
        scores.reserve(moves_.size());

        for (auto&& move : moves_) {
            make_move(move);
            scores.push_back(evaluate(color));
            undo_move();
        }

        for (auto i = 0; i < std::min(moves_.size(), 5U); i++) {
            auto max = std::numeric_limits<int>::min();
            auto location = -1;

            for (auto j = 0; j < scores.size(); j++) {
                if (scores[j] > max) {
                    max = scores[j];
                    location = j;
                }
            }

            if (location != -1) {
                scores[location] = std::numeric_limits<int>::min();
                swap(moves_[i], moves_[location]);
            }
        }
    }

    void executor::sort_captures() {
        // Move sorting
        auto color = board_->active_color();
        std::vector<int> scores;
        std::vector<std::shared_ptr<move>> sorted;
        sorted.reserve(captures_.size());
        scores.reserve(captures_.size());

        for (auto&& move : captures_) {
            make_move(move);
            scores.push_back(evaluate(color));
            undo_move();
        }

        for (auto i = 0; i < std::min(captures_.size(), 5U); i++) {
            auto max = std::numeric_limits<int>::min();
            auto location = -1;

            for (auto j = 0; j < scores.size(); j++) {
                if (scores[j] > max) {
                    max = scores[j];
                    location = j;
                }
            }

            if (location != -1) {
                scores[location] = std::numeric_limits<int>::min();
                swap(captures_[i], captures_[location]);
            }
        }
    }

    std::vector<std::shared_ptr<move>> executor::allowed_moves() const {
        return moves_;
    }

    std::vector<std::shared_ptr<move>> executor::allowed_captures() const {
        return captures_;
    }

    int executor::evaluate() const {
        return evaluate(black);
    }

    int executor::evaluate(piece_color color) const {
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

        switch (color) {
            case white:
                return material[white] - material[black]
                    + attack[white] - attack[black]
                    + defense[white] - defense[black]
                    + position[white] - position[black];
            case black:
                return material[black] - material[white]
                    + attack[black] - attack[white]
                    + defense[black] - defense[white]
                    + position[black] - position[white];
            default:
                return 0;
        }
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

    std::shared_ptr<board> executor::board() const {
        return board_;
    }

    bool executor::terminal() const {
        return terminal_;
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
