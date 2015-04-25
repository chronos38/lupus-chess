#pragma once
#include "enum.h"
#include <memory>
#include <vector>
#include <array>

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

class ipiece {
public:
    virtual ~ipiece() = default;
    virtual int score() const = 0;
    virtual int attack_score() const = 0;
    virtual int defense_score() const = 0;
    virtual int position_score() const = 0;
    virtual std::vector<std::shared_ptr<move>> allowed_moves() const = 0;
    virtual piece_value value() const = 0;
    virtual piece_type type() const = 0;
    virtual piece_color color() const = 0;
    virtual const char* position() const = 0;
    virtual std::shared_ptr<board> board() const = 0;
};

class piece : public ipiece {
public:
    piece() = default;
    piece(piece&& other);
    piece(const piece& other);
    piece(std::shared_ptr<::board> board, piece_value value);
    piece(std::shared_ptr<::board> board, piece_color color, piece_type type);
    virtual ~piece() = default;
    virtual int score() const override;
    virtual int attack_score() const override;
    virtual int defense_score() const override;
    virtual int position_score() const override;
    virtual std::vector<std::shared_ptr<move>> allowed_moves() const override;
    virtual piece_value value() const override;
    virtual piece_type type() const override;
    virtual piece_color color() const override;
    virtual const char* position() const override;
    virtual std::shared_ptr<::board> board() const override;
    virtual piece& operator=(piece&& other);
    virtual piece& operator=(const piece& other);
private:
    std::array<char, 3> position_;
    std::shared_ptr<::board> board_;
    std::unique_ptr<piece_state> state_;
};
