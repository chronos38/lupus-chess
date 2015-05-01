#include "view.h"
#include <chess/enum.h>
#include <chess/board.h>
#include <chess/piece.h>
#include <chess/move.h>
#include <sstream>
#include <unordered_map>
#include <iostream>

using namespace chess;

static const std::unordered_map<uint8_t, std::string> piece_map = {
    { white_pawn, "WP" },
    { white_bishop, "WB" },
    { white_knight, "WN" },
    { white_rook, "WR" },
    { white_queen, "WQ" },
    { white_king, "WK" },
    { black_pawn, "BP" },
    { black_bishop, "BB" },
    { black_knight, "BN" },
    { black_rook, "BR" },
    { black_queen, "BQ" },
    { black_king, "BK" }
};

void print_board(std::shared_ptr<board> board) {
    for (auto rank = 8; rank > 0; rank--) {
        std::cout << "|";
        for (auto i = 0; i < 8; i++)
            std::cout << "--|";
        std::cout << "\n|";
        for (auto file = 'a'; file != 'i'; file++) {
            if (board->get(file, rank))
                std::cout << piece_map.at(board->get(file, rank));
            else
                std::cout << "  ";
            std::cout << "|";
        }
        std::cout << "\n";
    }
     
    std::cout << "|";
    for (auto i = 0; i < 8; i++)
        std::cout << "--|";
    std::cout << std::endl;
}

void print_available_moves(std::shared_ptr<ipiece> piece) {
    std::stringstream ss;
    for (auto&& move : piece->allowed_moves()) {
        if (ss.str().length())
            ss << ", " << move->to_string();
        else
            ss << move->to_string();
    }
    
    std::cout << "Moves: " << ss.str() << std::endl;
}

std::string handle_square_selection() {
    std::string result;
    std::string input;

    do {
        std::cout << "Select a file: ";
    } while ((input = read_line()).size() != 1 || input.find_first_not_of("abcdefghABCDEFGH") != input.npos);
    result += tolower(input[0]);
    do {
        std::cout << "Select a rank: ";
    } while ((input = read_line()).size() != 1 || input.find_first_not_of("12345678") != input.npos);
    return result + input;
}

std::string read_line() {
    std::string result;
    auto ch = 0;

    while ((ch = getchar()) != '\n')
        result += ch;
    return result;
}
