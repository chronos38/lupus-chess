#pragma once
#include "enum.h"
#include "array.hpp"
#include <memory>

namespace chess {
    class move;

    class board make_board(const char* fen);
    class std::shared_ptr<board> make_shared_board(const char* fen);

    class board : public array_2d<uint8_t, 8, 8> {
    public:
        board();
        board(board&& other);
        board(const board& other);
        virtual ~board() = default;
        uint8_t get(size_t index) const;
        uint8_t get(char file, uint8_t rank) const;
        uint8_t get(const char* position) const;
        void set(size_t index, uint8_t value);
        void set(char file, uint8_t rank, uint8_t value);
        void set(const char* position, uint8_t value);
        int count();
        std::string to_fen() const;
        piece_color active_color() const;
        void toggle_active_color();
        const char* castling() const;
        void set_castling(const char* value);
        const char* en_passant() const;
        void set_en_passant(const char* value);
        uint8_t halfmove() const;
        void set_halfmove(uint8_t value);
        uint8_t fullmove() const;
        void set_fullmove(uint8_t value);
        board& operator=(board&& other);
        board& operator=(const board& other);
        static board create_starting_board();
        static std::shared_ptr<board> create_shared_starting_board();
    private:
        friend board make_board(const char*);
        std::array<char, 5> castling_;
        std::array<char, 3> en_passant_;
        piece_color active_ = white;
        uint8_t halfmove_ = 0;
        uint8_t fullmove_ = 0;
    };
}
