#include "move.h"
#include "board.h"
#include "piece.h"

move::move() {
    from_.fill(0);
    to_.fill(0);
}

move::move(move&& other) {
    swap(piece_, other.piece_);
    swap(board_, other.board_);
    swap(from_, other.from_);
    swap(to_, other.to_);
}

move::move(const move& other) {
    piece_ = other.piece_;
    board_ = other.board_;
    from_ = other.from_;
    to_ = other.to_;
}

move::move(std::shared_ptr<piece> piece, castling castling) : piece_(piece), board_(piece->board()), castling_(castling) {
}

move::move(std::shared_ptr<piece> piece, const char* from, const char* to) : piece_(piece), board_(piece->board()) {
    auto from_length = strlen(from);
    auto to_length = strlen(to);

    if (from_length >= from_.max_size())
        from_length = from_.max_size() - 1;
    if (to_length >= to_.max_size())
        to_length = to_.max_size() - 1;

    memcpy(from_.data(), from, from_length);
    memcpy(to_.data(), to, to_length);
}

void move::execute() {
    throw std::exception("void move::exectue() not implemented");
}

void move::undo() {
    throw std::exception("void move::undo() not implemented");
}

std::string move::to_string() const {
    throw std::exception("std::string move::to_string() not implemented");
    std::string result;
    result.reserve(16);

    if (piece_->type() != pawn) {
        result += toupper(piece_->value());
    } else {
    }

    result.shrink_to_fit();
    return result;
}

move& move::operator=(move&& other) {
    swap(piece_, other.piece_);
    swap(board_, other.board_);
    swap(from_, other.from_);
    swap(to_, other.to_);
    other.piece_ = nullptr;
    other.board_ = nullptr;
    from_.fill(0);
    to_.fill(0);
    return *this;
}

move& move::operator=(const move& other) {
    piece_ = other.piece_;
    board_ = other.board_;
    from_ = other.from_;
    to_ = other.to_;
    return *this;
}
