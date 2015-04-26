#pragma once
#include "enum.h"
#include "array.hpp"
#include <memory>
#include <vector>
#include <array>
#include <unordered_map>

namespace chess {
    static const int end_game_transition_count = 11;
    static const std::unordered_map<piece_type, int> attack_score_map = {
        { king, 2000 },
        { queen, 100 },
        { knight, 35 },
        { bishop, 35 },
        { rook, 50 },
        { pawn, 10 }
    };
    static const std::unordered_map<piece_type, int> defense_score_map = {
        { king, 0 },
        { queen, 100 },
        { knight, 35 },
        { bishop, 35 },
        { rook, 50 },
        { pawn, 10 }
    };
    static const array_2d<int, 8, 8> center_position_score_array = {
        -15, -15, -15, -15, -15, -15, -15, -15,
        -15, 0, 0, 0, 0, 0, 0, -15,
        -15, 0, 10, 10, 10, 10, 0, -15,
        -15, 0, 10, 25, 25, 10, 0, -15,
        -15, 0, 10, 25, 25, 10, 0, -15,
        -15, 0, 10, 10, 10, 10, 0, -15,
        -15, 0, 0, 0, 0, 0, 0, -15,
        -15, -15, -15, -15, -15, -15, -15, -15
    };
    static const array_2d<int, 8, 8> king_position_score_array = {
        55, 45, 25, 10, 10, 25, 45, 55,
        10, 5, 0, -5, -5, 0, 5, 10,
        -10, -10, -10, -15, -15 -10, -10, -10,
        -25, -25, -25, -35, -35, -25, -25, -25,
        -35, -35, -35, -45, -45, -35, -35, -35,
        -50, -50, -50, -50, -50, -50, -50, -50,
        -100, -100, -100, -100, -100, -100, -100, -100,
        -200, -200, -200, -200, -200, -200, -200, -200
    };
    static const array_2d<int, 8, 8> pawn_position_score_array = {
        0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0,
        5, 5, 5, 5, 5, 5, 5, 5,
        5, 5, 7, 10, 10, 7, 5, 5,
        10, 10, 10, 15, 15, 10, 10, 10,
        15, 15, 15, 15, 15, 15, 15, 15,
        20, 20, 20, 20, 20, 20, 20, 20,
        100, 100, 100, 100, 100, 100, 100, 100
    };

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
        virtual std::shared_ptr<board> board() const = 0;
    protected:
        friend class piece_state;
        std::unique_ptr<piece_state> state_;
    };

    class piece_state {
    public:
        piece_state() = default;
        explicit piece_state(piece_color color);
        virtual ~piece_state() = default;
        int score() const;
        int attack_score() const;
        int defense_score() const;
        int position_score() const;
        std::vector<std::shared_ptr<move>> allowed_moves() const;
        const char* position() const;
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
        piece(std::shared_ptr<chess::board> board, piece_value value);
        piece(std::shared_ptr<chess::board> board, piece_color color, piece_type type);
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
        virtual std::shared_ptr<chess::board> board() const override;
        virtual piece& operator=(piece&& other);
        virtual piece& operator=(const piece& other);
    private:
        friend class piece_state;
        std::shared_ptr<chess::board> board_;
        std::unique_ptr<piece_state> state_;
    };


    const char* piece_position(std::shared_ptr<board> on_board, piece_value for_value, std::array<char, 3>& position);
}
