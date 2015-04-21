#include "board.h"
#include <exception>
#include <locale>
#include <algorithm>

board make_board(const char* fen)
{
    board result;
    auto file = 'a';
    auto rank = 8;
    auto length = strlen(fen);
    auto space = std::find(fen, fen + length, ' ');
    static auto copy = [&] (char* ref) {
        if (*(space += 2) != '-') {
            auto find = std::find(space, fen + length, ' ');
            memcpy(ref, space, find - space);
            space = find - 1;
        } else {
            ref[0] = '-';
        }
    };
    static auto convert = [&] () {
        auto tenner = *(space += 2) - '0';
        
        if (space + 1 < fen + length && isdigit(*(space + 1))) {
            return tenner * 10 + (*(space += 1) - '0');
        } else {
            return tenner;
        }
    };
    
    // Board
    for (auto it = fen, end = fen + length; it < end; it++) {
        auto ch = *it;

        if (ch == '/') {
            if (rank-- == 0)
                break;
            file = 'a';
            continue;
        }
        if (ch == ' ') {
            break;
        }

        switch (ch) {
        case white_pawn:
        case black_pawn:
        case white_bishop:
        case black_bishop:
        case white_knight:
        case black_knight:
        case white_rook:
        case black_rook:
        case white_king:
        case black_king:
        case white_queen:
        case black_queen:
            result.set(file, rank, ch);
            file += 1;
            break;
        default:
            if (isdigit(ch))
                file += ch - '0';
            break;
        }
    }

    // Active color
    if (*(++space) == 'w') {
        result.active_ = white;
    } else {
        result.active_ = black;
    }

    // Castling
    copy(result.castling_);

    // En passant
    copy(result.en_passant_);

    // Halfmove clock
    result.halfmove_ = convert();

    // Fullmove clock
    result.fullmove_ = convert();

    return result;
}

std::shared_ptr<board> make_shared_board(const char* fen) {
    return std::make_shared<board>(std::move(make_board(fen)));
}

board::board() {
    memset(field_, 0, sizeof(field_));
    memset(castling_, 0, sizeof(castling_));
    memset(en_passant_, 0, sizeof(en_passant_));
}

board::board(board&& board) {
    std::swap(field_, board.field_);
    std::swap(active_, board.active_);
    std::swap(castling_, board.castling_);
    std::swap(en_passant_, board.en_passant_);
    std::swap(halfmove_, board.halfmove_);
    std::swap(fullmove_, board.fullmove_);
}

uint8_t* board::begin() {
    return field_;
}

const uint8_t* board::begin() const {
    return field_;
}

uint8_t* board::end() {
    return field_ + sizeof(field_);
}

const uint8_t* board::end() const {
    return field_ + sizeof(field_);
}

uint8_t board::get(char file, int rank) const {
    rank -= 1;
    auto index = tolower(file) - 'a';
    return field_[rank * 8 + index];
}

uint8_t board::get(const char* position) const {
    auto rank = std::stoi(std::string(1, position[1])) - 1;
    auto index = tolower(position[0]) - 'a';
    return field_[rank * 8 + index];
}

void board::set(char file, int rank, uint8_t value) {
    rank -= 1;
    auto index = tolower(file) - 'a';
    field_[rank * 8 + index] = value;
}

void board::set(const char* position, uint8_t value) {
    auto rank = std::stoi(std::string(1, position[1])) - 1;
    auto index = tolower(position[0]) - 'a';
    field_[rank * 8 + index] = value;
}

int board::count() {
    auto result = 0;
    for (auto&& position : field_)
        if (position)
            result++;
    return result;
}

std::string board::to_fen() const {
    // XXX: Check memory access since there is something wrong.
    std::string result;
    result.reserve(128);

    for (auto rank = 8; rank > 0; rank--) {
        auto count = 0;

        for (auto file = 'a'; file != 'i'; file++) {
            auto square = get(file, rank);

            if (square) {
                if (count) {
                    result += std::to_string(count);
                    count = 0;
                }

                result += square;
            } else {
                count++;
            }
        }

        if (count) {
            result += std::to_string(count);
            count = 0;
        }

        result += '/';
    }

    // Remove last slash
    result.back() = ' ';
    result += active_ == white ? 'w' : 'b';
    result += ' ';
    result += castling_;
    result += ' ';
    result += en_passant_;
    result += ' ';
    result += std::to_string(halfmove_);
    result += ' ';
    result += std::to_string(fullmove_);

    result.shrink_to_fit();
    return result;
}

piece_color board::active_color() const {
    return active_;
}

void board::toggle_active_color() {
    active_ = active_ == white ? black : white;
}

const char* board::castling() const {
    return castling_;
}

void board::set_castling(const char* value) {
    auto length = strlen(value);

    if (length < sizeof(castling_)) {
        memcpy(castling_, value, length);
    } else {
        throw std::length_error("board::set_castling(const char* value) allows a maximum string length of 4.");
    }
}

const char* board::en_passant() const {
    return en_passant_;
}

void board::set_en_passant(const char* value) {
    auto length = strlen(value);

    if (length < sizeof(en_passant_)) {
        memcpy(en_passant_, value, length);
    } else {
        throw std::length_error("board::set_castling(const char* value) allows a maximum string length of 2.");
    }
}

uint8_t board::halfmove() const {
    return halfmove_;
}

uint8_t board::fullmove() const {
    return fullmove_;
}

uint8_t& board::operator[](int index) {
    return field_[index];
}

const uint8_t& board::operator[](int index) const {
    return field_[index];
}

board board::create_starting_board() {
    return make_board("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
}

void board::set(int row, int column, uint8_t value) {
    field_[row * 8 + column] = value;
}
