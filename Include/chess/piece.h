﻿#pragma once
#include <cstdint>

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

class piece {
public:
    virtual ~piece() = default;
};