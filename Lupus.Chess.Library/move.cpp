#include "board.h"
#include "move.h"
#include <set>

move::move(move&& move) {
    std::swap(value_, move.value_);
}

move::move(board* board, const char* string) {
    auto length = strlen(string);

    if (length >= sizeof(value_)) {
        throw std::length_error("move does support only strings with a maximum length of 8.");
    }

    memcpy(value_, string, length);
    board_ = board;
}

void move::execute() {
    throw std::exception("move::exectue not implemented");
}

void move::undo() {
    throw std::exception("move::undo not implemented");
}

bool move::validate() const {
    throw std::exception("move::validate not implemented");
}