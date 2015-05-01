#include <iostream>
#include <string>
#include <chess/enum.h>
#include <chess/board.h>
#include <chess/piece.h>
#include <chess/move.h>
#include <chess/search.h>
#include <chess/executor.h>
#include "view.h"

using std::cout;
using std::endl;
using std::string;
using std::make_shared;
using namespace chess;

int main(int argc, char** argv) {
    string input;
    auto board = board::create_shared_starting_board();

    cout << "Welcome to LupusChess." << endl;
    cout << "Press ^C to exit the program (SIGINT).\n" << endl;
    print_board(board);

    while (true) {
        cout << "Select a piece from square." << endl;
        auto from = handle_square_selection();
        auto value = board->get(from.c_str());
        
        if (!value) {
            cout << "No piece available on that square." << endl;
            continue;
        }
        if (value_to_color(static_cast<piece_value>(value)) != white) {
            cout << "Only white pieces are available." << endl;
            continue;
        }

        auto piece = make_shared<chess::piece>(board, from.c_str());
        cout << "Where to move the piece to?" << endl;
        auto to = handle_square_selection();
        auto move = make_shared<chess::move>(piece.get(), from.c_str(), to.c_str());
        auto valid = false;

        piece->update();
        for (auto&& valid_move : piece->allowed_moves()) {
            if (*valid_move == *move) {
                valid = true;
                break;
            }
        }

        if (!valid) {
            cout << "Invalid move." << endl;
            continue;
        }

        move->execute();
        print_board(board);
        cout << "AI move..." << endl;
        board->toggle_active_color();
        auto exe = make_shared<executor>(board);
        auto ai = iterative_search(exe, 2);
        ai->set_board(board);
        ai->execute();
        print_board(board);
        board->toggle_active_color();
    }
}
