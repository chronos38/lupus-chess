#include "board.h"
#include <string>

namespace chess {
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
        copy(result.castling_.data());

        // En passant
        copy(result.en_passant_.data());

        // Halfmove clock
        result.halfmove_ = convert();

        // Fullmove clock
        result.fullmove_ = convert();

        return result;
    }

    std::shared_ptr<board> make_shared_board(const char* fen) {
        return std::make_shared<board>(std::move(make_board(fen)));
    }

    board::board() : array_2d<uint8_t, 8, 8>() {
        castling_.fill(0);
        en_passant_.fill(0);
    }

    board::board(board&& other) : array_2d<uint8_t, 8, 8>(std::forward<array_2d&&>(other)) {
        swap(castling_, other.castling_);
        swap(en_passant_, other.en_passant_);
        active_ = other.active_;
        halfmove_ = other.halfmove_;
        fullmove_ = other.fullmove_;

        other.active_ = white;
        other.halfmove_ = 0;
        other.fullmove_ = 1;
    }

    board::board(const board& other) : array_2d<uint8_t, 8, 8>(other) {
        castling_ = other.castling_;
        en_passant_ = other.en_passant_;
        active_ = other.active_;
        halfmove_ = other.halfmove_;
        fullmove_ = other.fullmove_;
    }

    std::shared_ptr<board> board::clone() const {
        return std::make_shared<board>(*this);
    }

    uint8_t board::get(size_t index) const {
        return array_2d::get(index);
    }

    uint8_t board::get(char file, uint8_t rank) const {
        auto index = tolower(file) - 'a';
        return array_2d::get(--rank * 8 + index);
    }

    uint8_t board::get(const char* position) const {
        return get(position[0], position[1] - '0');
    }

    void board::set(size_t index, uint8_t value) {
        array_2d::set(index, value);
    }

    void board::set(char file, uint8_t rank, uint8_t value) {
        auto index = tolower(file) - 'a';
        array_2d::set(--rank * 8 + index, value);
    }

    void board::set(const char* position, uint8_t value) {
        set(position[0], position[1] - '0', value);
    }

    int board::count() {
        auto result = 0;
        for (auto i = 63; i >= 0; i--)
            if (array_2d::get(i))
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
        result += castling_.data();
        result += ' ';
        result += en_passant_.data();
        result += ' ';
        result += std::to_string(halfmove_);
        result += ' ';
        result += std::to_string(fullmove_);

        return result;
    }

    piece_color board::active_color() const {
        return active_;
    }

    void board::toggle_active_color() {
        active_ = active_ == white ? black : white;
    }

    const char* board::castling() const {
        return castling_.data();
    }

    void board::set_castling(const char* value) {
        auto length = strlen(value);
        if (length >= castling_.max_size())
            length = castling_.max_size() - 1;
        memset(castling_.data(), 0, length + 1);
        memcpy(castling_.data(), value, length);
    }

    const char* board::en_passant() const {
        return en_passant_.data();
    }

    void board::set_en_passant(const char* value) {
        auto length = strlen(value);
        if (length >= en_passant_.max_size())
            length = en_passant_.max_size() - 1;
        memset(en_passant_.data(), 0, length + 1);
        memcpy(en_passant_.data(), value, length);
    }

    uint8_t board::halfmove() const {
        return halfmove_;
    }

    void board::set_halfmove(uint8_t value) {
        halfmove_ = value;
    }

    uint8_t board::fullmove() const {
        return fullmove_;
    }

    void board::set_fullmove(uint8_t value) {
        fullmove_ = value;
    }

    board& board::operator=(board&& other) {
        if (this != &other) {
            array_2d::operator=(other);
            swap(castling_, other.castling_);
            swap(en_passant_, other.en_passant_);
            active_ = other.active_;
            halfmove_ = other.halfmove_;
            fullmove_ = other.fullmove_;

            other.active_ = white;
            other.halfmove_ = 0;
            other.fullmove_ = 1;
            other.castling_.fill(0);
            other.en_passant_.fill(0);
        }

        return *this;
    }

    board& board::operator=(const board& other) {
        array_2d::operator=(other);
        castling_ = other.castling_;
        en_passant_ = other.en_passant_;
        active_ = other.active_;
        halfmove_ = other.halfmove_;
        fullmove_ = other.fullmove_;
        return *this;
    }

    board board::create_starting_board() {
        return make_board("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
    }

    std::shared_ptr<board> board::create_shared_starting_board() {
        return make_shared_board("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
    }
}