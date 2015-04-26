#pragma once
#include "enum.h"
#include "array.hpp"
#include <memory>
#include <vector>
#include <array>
#include <unordered_map>

namespace chess {
    class board;
    class move;
    class piece;

    class ipiece {
    public:
        virtual ~ipiece() = default;
        virtual int score() const = 0;
        virtual void update() = 0;
        virtual int attack_score() const = 0;
        virtual int defense_score() const = 0;
        virtual int position_score() const = 0;
        virtual std::vector<std::shared_ptr<move>> allowed_moves() const = 0;
        virtual piece_value value() const = 0;
        virtual piece_type type() const = 0;
        virtual piece_color color() const = 0;
        virtual const char* position() const = 0;
        virtual void set_position(const char* value) = 0;
        virtual std::shared_ptr<board> board() const = 0;
    protected:
        friend class piece_state;
        std::unique_ptr<piece_state> state_;
    };

    class piece_state {
    public:
        piece_state() = default;
        explicit piece_state(const char* position, piece_color color);
        virtual ~piece_state() = default;
        int score() const;
        int attack_score() const;
        int defense_score() const;
        int position_score() const;
        std::vector<std::shared_ptr<move>> allowed_moves() const;
        const char* position() const;
        void set_position(const char* value);
        piece_color color() const;
        virtual std::unique_ptr<piece_state> clone() const = 0;
        virtual void update(ipiece* piece) = 0;
        virtual piece_value value() const = 0;
        virtual piece_type type() const = 0;
    protected:
        void change_state(ipiece* piece, std::unique_ptr<piece_state> state);
        std::vector<std::shared_ptr<move>> moves_;
        std::array<char, 3> position_;
        std::array<uint8_t, 8> collisions_;
        piece_color color_;
        int score_ = 0;
        int attack_score_ = 0;
        int defense_score_ = 0;
        int position_score_ = 0;
    };

    class piece : public ipiece {
    public:
        piece() = default;
        piece(piece&& other);
        piece(const piece& other);
        piece(std::shared_ptr<chess::board> board, const char* position);
        virtual ~piece() = default;
        virtual void update() override;
        virtual int score() const override;
        virtual int attack_score() const override;
        virtual int defense_score() const override;
        virtual int position_score() const override;
        virtual std::vector<std::shared_ptr<move>> allowed_moves() const override;
        virtual piece_value value() const override;
        virtual piece_type type() const override;
        virtual piece_color color() const override;
        virtual const char* position() const override;
        virtual void set_position(const char* value) override;
        virtual std::shared_ptr<chess::board> board() const override;
        virtual piece& operator=(piece&& other);
        virtual piece& operator=(const piece& other);
    private:
        friend class piece_state;
        std::shared_ptr<chess::board> board_;
    };


    const char* piece_position(std::shared_ptr<board> on_board, piece_value for_value, std::array<char, 3>& position);
}
