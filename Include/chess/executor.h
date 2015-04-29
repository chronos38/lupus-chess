#pragma once
#include "enum.h"
#include "undo_redo.h"
#include <memory>
#include <vector>
#include <deque>
#include <stack>

namespace chess {
    class board;
    class move;
    class piece;

    class executor {
    public:
        executor() = delete;
        executor(executor&& other);
        executor(const executor& other);
        explicit executor(std::shared_ptr<chess::board> board);
        ~executor() = default;
        std::shared_ptr<executor> clone() const;
        void update();
        void sort_moves();
        void sort_captures();
        std::vector<std::shared_ptr<move>> allowed_moves() const;
        std::vector<std::shared_ptr<move>> allowed_captures() const;
        int evaluate() const;
        int evaluate(piece_color color) const;
        void make_move(std::shared_ptr<move> move);
        void undo_move();
        std::shared_ptr<board> board() const;
        bool terminal() const;
        executor& operator=(executor&& other);
        executor& operator=(const executor& other);
    private:
        undo_redo_stack stack_;
        std::shared_ptr<chess::board> board_;
        std::deque<std::shared_ptr<piece>> pieces_;
        std::vector<std::shared_ptr<move>> moves_;
        std::vector<std::shared_ptr<move>> captures_;
        std::stack<std::shared_ptr<piece>> erased_;
        bool terminal_ = false;
    };
}
