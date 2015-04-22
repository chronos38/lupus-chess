#pragma once
#include <cstdint>
#include <memory>

enum piece_value {
    white_king = 'K',
    white_queen = 'Q',
    white_rook = 'R',
    white_knight = 'N',
    white_bishop = 'B',
    white_pawn = 'P',
    black_king = 'k',
    black_queen = 'q',
    black_rook = 'r',
    black_knight = 'n',
    black_bishop = 'b',
    black_pawn = 'p'
};

enum piece_color {
    white,
    black
};

enum piece_type {
    king,
    queen,
    rook,
    knight,
    bishop,
    pawn
};

class piece_state {
public:
};

class piece {
public:
    piece() = delete;
    explicit piece(piece_value value);
    piece(piece_color color, piece_type type);
    piece(piece&& other);
    piece(const piece& other);
    virtual ~piece() = default;
    piece& operator=(piece&& other);
    piece& operator=(const piece& other);
private:
    std::unique_ptr<piece_state> state_;
};
