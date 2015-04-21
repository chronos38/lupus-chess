#pragma once
#include <cstdint>
#include <string>
#include "piece.h"

class board make_board(const char* fen);

enum castling {
    both_sides,
    king_side,
    queen_side,
    no_side
};

class board {
public:
    board();
    board(board&& board);
    board(const board&) = default;
    virtual ~board() = default;
    uint8_t* begin();
    const uint8_t* begin() const;
    uint8_t* end();
    const uint8_t* end() const;
    uint8_t get(char file, int rank) const;
    uint8_t get(const char* position) const;
    void set(char file, int rank, uint8_t value);
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
    uint8_t fullmove() const;
    uint8_t& operator[](int index);
    const uint8_t& operator[](int index) const;
    static board create_starting_board();
private:
    friend board make_board(const char*);
    void set(int row, int column, uint8_t value);
    uint8_t field_[64];
    piece_color active_ = white;
    char castling_[5];
    char en_passant_[3];
    uint8_t halfmove_ = 0;
    uint8_t fullmove_ = 0;
};
