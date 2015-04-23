#pragma once
#include <memory>
#include <vector>

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

class board;
class move;
class piece;

class piece_state {
public:
    virtual ~piece_state() = default;
    virtual std::unique_ptr<piece_state> clone() const = 0;
    virtual int score(const piece* piece) const = 0;
    virtual int attack_score(const piece* piece, std::shared_ptr<board> board) const = 0;
    virtual int defense_score(const piece* piece, std::shared_ptr<board> board) const = 0;
    virtual int position_score(const piece* piece, std::shared_ptr<board> board) const = 0;
    virtual std::vector<std::shared_ptr<move>> allowed_moves(const piece* piece, std::shared_ptr<board> board) const = 0;
    virtual piece_value value(const piece* piece) const = 0;
    virtual piece_type type(const piece* piece) const = 0;
    virtual piece_color color(const piece* piece) const = 0;
};

class piece {
public:
    piece() = default;
    explicit piece(std::shared_ptr<board> board, piece_value value);
    explicit piece(std::shared_ptr<board> board, piece_color color, piece_type type);
    piece(piece&& other);
    piece(const piece& other);
    virtual ~piece() = default;
    int score() const;
    int attack_score() const;
    int defense_score() const;
    int position_score() const;
    std::vector<std::shared_ptr<move>> allowed_moves() const;
    piece_value value() const;
    piece_type type() const;
    piece_color color() const;
    std::shared_ptr<board> board() const;
    void set_board(std::shared_ptr<::board> value);
    piece& operator=(piece&& other);
    piece& operator=(const piece& other);
private:
    std::shared_ptr<::board> board_;
    std::unique_ptr<piece_state> state_;
};
