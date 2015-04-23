#include "piece.h"
#include "board.h"
#include "move.h"

class piece_king : public piece_state {
public:
    piece_king(piece_color color);
    virtual ~piece_king() = default;
    virtual std::unique_ptr<piece_state> clone() const override;
    virtual int score(const piece* piece) const override;
    virtual int attack_score(const piece* piece, std::shared_ptr<board> board) const override;
    virtual int defense_score(const piece* piece, std::shared_ptr<board> board) const override;
    virtual int position_score(const piece* piece, std::shared_ptr<board> board) const override;
    virtual std::vector<std::shared_ptr<move>> allowed_moves(const piece* piece, std::shared_ptr<board> board) const override;
    virtual piece_value value(const piece* piece) const override;
    virtual piece_type type(const piece* piece) const override;
    virtual piece_color color(const piece* piece) const override;
private:
    piece_color color_;
};

class piece_queen : public piece_state {
public:
    piece_queen(piece_color color);
    virtual ~piece_queen() = default;
    virtual std::unique_ptr<piece_state> clone() const override;
    virtual int score(const piece* piece) const override;
    virtual int attack_score(const piece* piece, std::shared_ptr<board> board) const override;
    virtual int defense_score(const piece* piece, std::shared_ptr<board> board) const override;
    virtual int position_score(const piece* piece, std::shared_ptr<board> board) const override;
    virtual std::vector<std::shared_ptr<move>> allowed_moves(const piece* piece, std::shared_ptr<board> board) const override;
    virtual piece_value value(const piece* piece) const override;
    virtual piece_type type(const piece* piece) const override;
    virtual piece_color color(const piece* piece) const override;
private:
    piece_color color_;
};

class piece_rook : public piece_state {
public:
    piece_rook(piece_color color);
    virtual ~piece_rook() = default;
    virtual std::unique_ptr<piece_state> clone() const override;
    virtual int score(const piece* piece) const override;
    virtual int attack_score(const piece* piece, std::shared_ptr<board> board) const override;
    virtual int defense_score(const piece* piece, std::shared_ptr<board> board) const override;
    virtual int position_score(const piece* piece, std::shared_ptr<board> board) const override;
    virtual std::vector<std::shared_ptr<move>> allowed_moves(const piece* piece, std::shared_ptr<board> board) const override;
    virtual piece_value value(const piece* piece) const override;
    virtual piece_type type(const piece* piece) const override;
    virtual piece_color color(const piece* piece) const override;
private:
    piece_color color_;
};

class piece_bishop : public piece_state {
public:
    piece_bishop(piece_color color);
    virtual ~piece_bishop() = default;
    virtual std::unique_ptr<piece_state> clone() const override;
    virtual int score(const piece* piece) const override;
    virtual int attack_score(const piece* piece, std::shared_ptr<board> board) const override;
    virtual int defense_score(const piece* piece, std::shared_ptr<board> board) const override;
    virtual int position_score(const piece* piece, std::shared_ptr<board> board) const override;
    virtual std::vector<std::shared_ptr<move>> allowed_moves(const piece* piece, std::shared_ptr<board> board) const override;
    virtual piece_value value(const piece* piece) const override;
    virtual piece_type type(const piece* piece) const override;
    virtual piece_color color(const piece* piece) const override;
private:
    piece_color color_;
};

class piece_knight : public piece_state {
public:
    piece_knight(piece_color color);
    virtual ~piece_knight() = default;
    virtual std::unique_ptr<piece_state> clone() const override;
    virtual int score(const piece* piece) const override;
    virtual int attack_score(const piece* piece, std::shared_ptr<board> board) const override;
    virtual int defense_score(const piece* piece, std::shared_ptr<board> board) const override;
    virtual int position_score(const piece* piece, std::shared_ptr<board> board) const override;
    virtual std::vector<std::shared_ptr<move>> allowed_moves(const piece* piece, std::shared_ptr<board> board) const override;
    virtual piece_value value(const piece* piece) const override;
    virtual piece_type type(const piece* piece) const override;
    virtual piece_color color(const piece* piece) const override;
private:
    piece_color color_;
};

class piece_pawn : public piece_state {
public:
    piece_pawn(piece_color color);
    virtual ~piece_pawn() = default;
    virtual std::unique_ptr<piece_state> clone() const override;
    virtual int score(const piece* piece) const override;
    virtual int attack_score(const piece* piece, std::shared_ptr<board> board) const override;
    virtual int defense_score(const piece* piece, std::shared_ptr<board> board) const override;
    virtual int position_score(const piece* piece, std::shared_ptr<board> board) const override;
    virtual std::vector<std::shared_ptr<move>> allowed_moves(const piece* piece, std::shared_ptr<board> board) const override;
    virtual piece_value value(const piece* piece) const override;
    virtual piece_type type(const piece* piece) const override;
    virtual piece_color color(const piece* piece) const override;
private:
    piece_color color_;
};

piece::piece(std::shared_ptr<::board> board, piece_value value) : board_(board) {
    throw std::exception("piece::piece(std::shared_ptr<::board> board, piece_value value) not implemented");
    switch (value) {
        case white_king:
            break;
        case white_queen:
            break;
        case white_rook:
            break;
        case white_bishop:
            break;
        case white_knight:
            break;
        case white_pawn:
            break;
        case black_king:
            break;
        case black_queen:
            break;
        case black_rook:
            break;
        case black_bishop:
            break;
        case black_knight:
            break;
        case black_pawn:
            break;
    }
}

piece::piece(std::shared_ptr<::board> board, piece_color color, piece_type type) {
}

piece::piece(piece&& other) {
    swap(board_, other.board_);
    swap(state_, other.state_);
}

piece::piece(const piece& other) {
    board_ = other.board_;
    state_ = other.state_->clone();
}

int piece::score() const {
    return state_->score(this);
}

int piece::attack_score() const {
    return state_->attack_score(this, board_);
}

int piece::defense_score() const {
    return state_->defense_score(this, board_);
}

int piece::position_score() const {
    return state_->position_score(this, board_);
}

std::vector<std::shared_ptr<move>> piece::allowed_moves() const {
    return state_->allowed_moves(this, board_);
}

piece_value piece::value() const {
    return state_->value(this);
}

piece_type piece::type() const {
    return state_->type(this);
}

piece_color piece::color() const {
    return state_->color(this);
}

std::shared_ptr<board> piece::board() const {
    return board_;
}

void piece::set_board(std::shared_ptr<::board> value) {
    board_ = value;
}

piece& piece::operator=(piece&& other) {
    swap(board_, other.board_);
    swap(state_, other.state_);
    other.board_ = nullptr;
    other.state_ = nullptr;
    return *this;
}

piece& piece::operator=(const piece& other) {
    board_ = other.board_;
    state_ = other.state_->clone();
    return *this;
}
