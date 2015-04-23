#pragma once
#include "enum.h"
#include <memory>
#include <vector>

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
