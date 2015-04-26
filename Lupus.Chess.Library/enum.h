#pragma once
#include <stdexcept>

namespace chess {
    enum castling {
        both_sides,
        king_side,
        queen_side,
        no_side
    };

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

    inline piece_type value_to_type(piece_value value) {
        switch (value) {
            case white_pawn:
            case black_pawn:
                return pawn;
            case white_bishop:
            case black_bishop:
                return bishop;
            case white_knight:
            case black_knight:
                return knight;
            case white_rook:
            case black_rook:
                return rook;
            case white_queen:
            case black_queen:
                return queen;
            case white_king:
            case black_king:
                return king;
            default:
                throw std::invalid_argument("could not determine the type from given piece value");
        }
    }

    inline piece_color value_to_color(piece_value value) {
        switch (value) {
            case white_pawn:
            case white_bishop:
            case white_knight:
            case white_rook:
            case white_queen:
            case white_king:
                return white;
            case black_pawn:
            case black_bishop:
            case black_knight:
            case black_rook:
            case black_queen:
            case black_king:
                return black;
            default:
                throw std::invalid_argument("could not determine the color from given piece value");
        }
    }
}
