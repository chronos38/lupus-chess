#include "undo_redo.h"
#include "command.h"

namespace chess {
    void undo_redo_stack::push(std::shared_ptr<command> cmd) {
        auto empty = std::stack<std::shared_ptr<command>>();
        std::swap(empty, redo_stack_);
        undo_stack_.push(cmd);
    }

    void undo_redo_stack::undo() {
        if (undo_stack_.empty())
            return;

        auto cmd = undo_stack_.top();
        redo_stack_.push(cmd);
        undo_stack_.pop();
        cmd->undo();
    }

    void undo_redo_stack::redo() {
        if (redo_stack_.empty())
            return;

        auto cmd = redo_stack_.top();
        undo_stack_.push(cmd);
        redo_stack_.pop();
        cmd->undo();
    }

    void undo_redo_stack::clear() {
        while (!redo_stack_.empty())
            redo_stack_.pop();
        while (!undo_stack_.empty())
            undo_stack_.pop();
    }
}
