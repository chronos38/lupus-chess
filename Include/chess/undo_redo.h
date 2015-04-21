﻿#pragma once
#include <stack>
#include <memory>

class command;

class undo_redo_stack {
public:
    virtual ~undo_redo_stack() = default;
    void push(std::shared_ptr<command> cmd);
    void undo();
    void redo();
private:
    std::stack<std::shared_ptr<command>> undo_stack_;
    std::stack<std::shared_ptr<command>> redo_stack_;
};