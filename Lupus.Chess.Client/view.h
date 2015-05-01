#pragma once
#include <memory>
#include <string>

namespace chess {
    class board;
    class ipiece;
}

void print_board(std::shared_ptr<chess::board> board);
void print_available_moves(std::shared_ptr<chess::ipiece> exe);
std::string handle_square_selection();
std::string read_line();
