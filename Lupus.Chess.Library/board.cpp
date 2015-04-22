#include "board.h"
#include <exception>
#include <locale>
#include <algorithm>

board make_board(const char* fen) {
    board result;
    auto file = 'a';
    auto rank = 8;
    auto length = strlen(fen);
    auto space = std::find(fen, fen + length, ' ');
    auto copy = [&] (char* ref) {
        if (*(space += 2) != '-') {
            auto find = std::find(space, fen + length, ' ');
            memcpy(ref, space, find - space);
            space = find - 1;
        } else {
            ref[0] = '-';
        }
    };
    auto convert = [&] () {
        auto tenner = *(space += 2) - '0';
        
        if (space + 1 < fen + length && isdigit(*(space + 1))) {
            return tenner * 10 + (*(space += 1) - '0');
        } else {
            return tenner;
        }
    };
    
    // Board
    for (auto it = fen, end = fen + length; it < end && it != space; it++) {
        auto ch = *it;

        if (ch == '/') {
            if (rank-- == 0)
                break;
            file = 'a';
            continue;
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
    copy(result.castling_.get());

    // En passant
    copy(result.en_passant_.get());

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
    field_ = std::make_unique<uint8_t[]>(64);
    castling_ = std::make_unique<char[]>(5);
    en_passant_ = std::make_unique<char[]>(3);
    memset(field_.get(), 0, 64);
    memset(castling_.get(), 0, 5);
    memset(en_passant_.get(), 0, 3);
}

board::board(board&& other) : board() {
    field_.swap(other.field_);
    castling_.swap(other.castling_);
    en_passant_.swap(other.en_passant_);
    active_ = other.active_;
    halfmove_ = other.halfmove_;
    fullmove_ = other.fullmove_;

    other.active_ = white;
    other.halfmove_ = 0;
    other.fullmove_ = 1;
    other.field_.reset(nullptr);
    other.castling_.reset(nullptr);
    other.en_passant_.reset(nullptr);
}

board::board(const board& other) : board() {
    memcpy(field_.get(), other.field_.get(), 64);
    memcpy(castling_.get(), other.castling_.get(), 5);
    memcpy(en_passant_.get(), other.en_passant_.get(), 3);
    active_ = other.active_;
    halfmove_ = other.halfmove_;
    fullmove_ = other.fullmove_;
}

uint8_t* board::begin() {
    return field_.get();
}

const uint8_t* board::begin() const {
    return field_.get();
}

uint8_t* board::end() {
    return field_.get() + 64;
}

const uint8_t* board::end() const {
    return field_.get() + 64;
}

uint8_t board::get(char file, int rank) const {
    auto index = tolower(file) - 'a';
    return field_[--rank * 8 + index];
}

uint8_t board::get(const char* position) const {
    return get(position[0], position[1] - '0');
}

void board::set(char file, int rank, uint8_t value) {
    auto index = tolower(file) - 'a';
    field_[--rank * 8 + index] = value;
}

void board::set(const char* position, uint8_t value) {
    set(position[0], position[1] - '0', value);
}

int board::count() {
    auto result = 0;
    for (auto i = 63; i >= 0; i--)
        if (field_[i])
            result++;
    return result;
}

std::string board::to_fen() const {
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
    result += castling_.get();
    result += ' ';
    result += en_passant_.get();
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
    return castling_.get();
}

void board::set_castling(const char* value) {
    auto length = strlen(value);
    if (length > 4)
        length = 4;

    if (length < sizeof(castling_)) {
        memcpy(castling_.get(), value, length);
    } else {
        throw std::length_error("board::set_castling(const char* value) allows a maximum string length of 4.");
    }
}

const char* board::en_passant() const {
    return en_passant_.get();
}

void board::set_en_passant(const char* value) {
    auto length = strlen(value);
    if (length > 2) length = 2;

    if (length < sizeof(en_passant_)) {
        memcpy(en_passant_.get(), value, length);
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

board& board::operator=(board&& other) {
    if (this != &other) {
        field_.swap(other.field_);
        castling_.swap(other.castling_);
        en_passant_.swap(other.en_passant_);
        active_ = other.active_;
        halfmove_ = other.halfmove_;
        fullmove_ = other.fullmove_;

        other.active_ = white;
        other.halfmove_ = 0;
        other.fullmove_ = 1;
        other.field_.reset(nullptr);
        other.castling_.reset(nullptr);
        other.en_passant_.reset(nullptr);
    }

    return *this;
}

board& board::operator=(const board& other) {
    memcpy(field_.get(), other.field_.get(), 64);
    memcpy(castling_.get(), other.castling_.get(), 5);
    memcpy(en_passant_.get(), other.en_passant_.get(), 3);
    active_ = other.active_;
    halfmove_ = other.halfmove_;
    fullmove_ = other.fullmove_;
    return *this;
}

board board::create_starting_board() {
    return make_board("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
}

void board::set(int row, int column, uint8_t value) {
    field_[row * 8 + column] = value;
}
