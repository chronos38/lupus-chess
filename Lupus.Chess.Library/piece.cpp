#include "piece.h"

piece::piece(piece_value value) {
}

piece::piece(piece_color color, piece_type type) {
}

piece::piece(piece&& other) {
}

piece::piece(const piece& other) {
}

piece& piece::operator=(piece&& other) {
    return *this;
}

piece& piece::operator=(const piece& other) {
    return *this;
}
