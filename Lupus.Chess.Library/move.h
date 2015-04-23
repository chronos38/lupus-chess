#pragma once
#include "command.h"
#include <memory>

class board;
class piece;

class move : public command {
public:
    move();
    move(move&& other);
    move(const move& other);
    explicit move(std::shared_ptr<piece> piece, const char* from, const char* to);
    virtual ~move() = default;
    virtual void execute() override;
    virtual void undo() override;
    std::string to_string() const;
    move& operator=(move&& other);
    move& operator=(const move& other);
private:
    std::shared_ptr<piece> piece_;
    std::shared_ptr<board> board_;
    char from_[3];
    char to_[3];
};
