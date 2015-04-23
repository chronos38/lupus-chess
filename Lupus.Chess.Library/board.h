#pragma once
#include "enum.h"
#include <string>
#include <memory>
#include <array>

class move;

class board make_board(const char* fen);
class std::shared_ptr<board> make_shared_board(const char* fen);

class board {
public:
    board();
    board(board&& other);
    board(const board& other);
    virtual ~board() = default;
    std::array<uint8_t, 64>::iterator begin();
    std::array<uint8_t, 64>::const_iterator begin() const;
    std::array<uint8_t, 64>::iterator end();
    std::array<uint8_t, 64>::const_iterator end() const;
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
    void set_halfmove(uint8_t value);
    uint8_t fullmove() const;
    void set_fullmove(uint8_t value);
    uint8_t& operator[](int index);
    const uint8_t& operator[](int index) const;
    board& operator=(board&& other);
    board& operator=(const board& other);
    static board create_starting_board();
private:
    friend board make_board(const char*);
    void set(int row, int column, uint8_t value);
    std::array<uint8_t, 64> field_;
    std::array<char, 5> castling_;
    std::array<char, 3> en_passant_;
    piece_color active_ = white;
    uint8_t halfmove_ = 0;
    uint8_t fullmove_ = 0;
};
