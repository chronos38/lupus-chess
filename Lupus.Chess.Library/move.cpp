#include "move.h"
#include "board.h"
#include "piece.h"

class move_castling : public move_state {
public:
    move_castling(std::shared_ptr<board> board, castling castling, piece_color color)
        : board_(board), castling_(castling), color_(color) {
    }

    virtual ~move_castling() = default;

    virtual void execute(move* move) final override {
        auto rank = color_ == white ? 1 : 8;
        switch (castling_) {
            case king_side:
                board_->set('e', rank, 0);
                board_->set('h', rank, 0);
                board_->set('g', rank, white_king);
                board_->set('f', rank, white_rook);
                break;
            case queen_side:
                board_->set('e', rank, 0);
                board_->set('a', rank, 0);
                board_->set('c', rank, white_king);
                board_->set('d', rank, white_rook);
                break;
            default:
                break;
        }
    }

    virtual void undo(move* move) final override {
        auto rank = color_ == white ? 1 : 8;
        switch (castling_) {
            case king_side:
                board_->set('g', rank, 0);
                board_->set('f', rank, 0);
                board_->set('e', rank, white_king);
                board_->set('h', rank, white_rook);
                break;
            case queen_side:
                board_->set('c', rank, 0);
                board_->set('d', rank, 0);
                board_->set('e', rank, white_king);
                board_->set('a', rank, white_rook);
                break;
            default:
                break;
        }
    }
    
    virtual std::string to_string(const move* move) const final override {
        switch (castling_) {
            case king_side:
                return "O-O";
            case queen_side:
                return "O-O-O";
            default:
                return "";
        }
    }
    
    virtual bool operator==(const move_state& other) const override {
        auto o = dynamic_cast<const move_castling*>(std::addressof(other));
        return o && castling_ == o->castling_ && color_ == o->color_;
    }

    virtual bool operator!=(const move_state& other) const override {
        return !(*this == other);
    }

private:

    std::shared_ptr<board> board_;
    castling castling_ = no_side;
    piece_color color_ = white;
};

class move_piece : public move_state {
public:
    move_piece(const ipiece* piece, const char* from, const char* to) : piece_(piece) {
        auto from_length = strlen(from);
        auto to_length = strlen(to);
        from_.fill(0);
        to_.fill(0);

        if (from_length >= from_.max_size())
            from_length = from_.max_size() - 1;
        if (to_length >= to_.max_size())
            to_length = to_.max_size();

        std::copy_n(from, from_length, std::begin(from_));
        std::copy_n(to, to_length, std::begin(to_));
        board_ = piece->board();
    }

    virtual ~move_piece() = default;
    
    virtual void execute(move* move) override {
        auto piece = board_->get(from_.data());
        if (!piece)
            return;
        if (piece != piece_->value())
            return;
        auto to = board_->get(to_.data());
        if (to)
            captured_ = to;

        board_->set(from_.data(), 0);
        board_->set(to_.data(), piece);
    }
    
    virtual void undo(move* move) override {
        auto piece = board_->get(to_.data());
        if (!piece)
            return;
        if (piece != piece_->value())
            return;
        board_->set(to_.data(), captured_);
        board_->set(from_.data(), piece);
        if (captured_)
            captured_ = 0;
    }
    
    virtual std::string to_string(const move* move) const override {
        std::string result;
        result.reserve(16);

        if (piece_->type() != pawn)
            result += toupper(piece_->value());
        result += piece_->position()[0];

        if (captured_)
            result += 'x';
        result += to_.data();

        if (piece_->type() == pawn && strcmp(to_.data(), board_->en_passant()) == 0)
            result += "e.p.";
        result.shrink_to_fit();
        return result;
    }

    virtual bool operator==(const move_state& other) const override {
        auto o = dynamic_cast<const move_piece*>(std::addressof(other));
        return o && piece_ == o->piece_ && from_ == o->from_ && to_ == o->to_ && captured_ == o->captured_;
    }

    virtual bool operator!=(const move_state& other) const override {
        return !(*this == other);
    }

protected:

    const ipiece* piece_ = nullptr;
    std::shared_ptr<board> board_;
    std::array<char, 3> from_;
    std::array<char, 3> to_;
    uint8_t captured_ = 0;
};

class move_en_passant : public move_piece {
public:
    move_en_passant(const ipiece* piece, const char* from, const char* to) : move_piece(piece, from, to) {
        auto en_passant = board_->en_passant();
        file_ = en_passant[0];
        rank_ = atoi(&en_passant[1]);
    }

    virtual ~move_en_passant() = default;

    virtual void execute(move* move) override {
        move_piece::execute(move);

        switch (rank_) {
            case 3:
                board_->set(file_, 4, 0);
                captured_ = white_pawn;
                break;
            case 6:
                board_->set(file_, 5, 0);
                captured_ = black_pawn;
                break;
        }
    }

    virtual void undo(move* move) override {
        switch (rank_) {
            case 3:
                board_->set(file_, 4, white_pawn);
                captured_ = 0;
                break;
            case 6:
                board_->set(file_, 5, black_pawn);
                break;
        }

        move_piece::undo(move);
    }

private:
    char file_ = 0;
    uint8_t rank_ = 0;
};

move::move(std::shared_ptr<board> board, castling castling, piece_color color) {
    state_ = std::make_unique<move_castling>(board, castling, color);
}

move::move(const ipiece* piece, const char* from, const char* to) {
    if (piece->type() == pawn && strcmp(to, piece->board()->en_passant()) == 0)
        state_ = std::make_unique<move_en_passant>(piece, from, to);
    else
        state_ = std::make_unique<move_piece>(piece, from, to);
}

void move::execute() {
    state_->execute(this);
}

void move::undo() {
    state_->undo(this);
}

std::string move::to_string() const {
    return state_->to_string(this);
}

bool move::operator==(const move& other) const {
    return *state_ == *other.state_;
}

bool move::operator!=(const move& other) const {
    return !(*this == other);
}
