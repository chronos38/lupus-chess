#include "board.h"
#include "move.h"
#include <set>

move::move() {
    memset(from_, 0, sizeof(from_));
    memset(to_, 0, sizeof(to_));
}

move::move(move&& move) {
}

move::move(std::shared_ptr<piece> piece, const char* from, const char* to) {
}

void move::execute() {
    throw std::exception("move::exectue not implemented");
}

void move::undo() {
    throw std::exception("move::undo not implemented");
}

std::string move::to_string() const {
    throw std::exception("move::to_string not implemented");
}

move& move::operator=(move&& other) {
    return *this;
}

move& move::operator=(const move& other) {
    return *this;
}
