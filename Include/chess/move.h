#pragma once
#include "command.h"
#include "enum.h"
#include <memory>

namespace chess {
    class board;
    class ipiece;
    class move;

    class move_state {
    public:
        virtual ~move_state() = default;
        virtual void execute(move* move) = 0;
        virtual void undo(move* move) = 0;
        virtual std::string to_string(const move* move) const = 0;
        virtual bool operator==(const move_state& other) const = 0;
        virtual bool operator!=(const move_state& other) const = 0;
    };

    class move : public command {
    public:
        move() = default;
        move(move&& other) = delete;
        move(const move& other) = delete;
        move(std::shared_ptr<board> board, castling castling, piece_color color, ipiece* king, ipiece* rook);
        move(ipiece* piece, const char* from, const char* to);
        virtual ~move() = default;
        virtual void execute() override;
        virtual void undo() override;
        virtual std::string to_string() const;
        virtual bool operator==(const move& other) const;
        virtual bool operator!=(const move& other) const;
    private:
        friend class move_state;
        std::unique_ptr<move_state> state_;
    };
}
