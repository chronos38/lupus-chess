#include "board.h"
#include "piece.h"

board::board()
{
    memset(field_, 0, sizeof(field_));
}

board::board(board&& board)
{
    memmove(field_, board.field_, sizeof(field_));
}

uint8_t board::get(char file, int rank)
{
    rank -= 1;
    auto index = tolower(file) - 'a';
    return field_[rank * 8 + index];
}

uint8_t board::get(const std::string& position)
{
    auto rank = std::stoi(std::string(1, position[1])) - 1;
    auto index = tolower(position[0]) - 'a';
    return field_[rank * 8 + index];
}

void board::set(char file, int rank, uint8_t value)
{
    rank -= 1;
    auto index = tolower(file) - 'a';
    field_[rank * 8 + index] = value;
}

void board::set(const std::string& position, uint8_t value)
{
    auto rank = std::stoi(std::string(1, position[1])) - 1;
    auto index = tolower(position[0]) - 'a';
    field_[rank * 8 + index] = value;
}

int board::count()
{
    auto result = 0;
    for (auto&& position : field_)
        if (position)
            result++;
    return result;
}

uint8_t& board::operator[](int index)
{
    return field_[index];
}

const uint8_t& board::operator[](int index) const
{
    return field_[index];
}

board board::create_start_board()
{
    board board;
    
    for (auto i = 0; i < 8; i++)
        board.set(1, i, WhitePawn);
    board.set(0, 0, WhiteRook);
    board.set(0, 7, WhiteRook);
    board.set(0, 1, WhiteKnight);
    board.set(0, 6, WhiteKnight);
    board.set(0, 2, WhiteBishop);
    board.set(0, 5, WhiteBishop);
    board.set(0, 3, WhiteQueen);
    board.set(0, 4, WhiteKing);

    for (auto i = 0; i < 8; i++)
        board.set(6, i, BlackPawn);
    board.set(7, 0, BlackRook);
    board.set(7, 7, BlackRook);
    board.set(7, 1, BlackKnight);
    board.set(7, 6, BlackKnight);
    board.set(7, 2, BlackBishop);
    board.set(7, 5, BlackBishop);
    board.set(7, 3, BlackQueen);
    board.set(7, 4, BlackKing);

    return board;
}

void board::set(int row, int column, uint8_t value)
{
    field_[row * 8 + column] = value;
}
