#pragma once
#include "command.h"
#include <memory>

class board;

class move : public command {
public:
    move(move&& move);
    explicit move(board* board, const char* string);
    move(const move&) = default;
    virtual ~move() = default;
    virtual void execute() override;
    virtual void undo() override;
    bool validate() const;
private:
    board* board_;
    char value_[9];
};
