#pragma once
#include <string>
#include <cstdint>

class board
{
public:
    board();
    board(board&& board);
    board(const board&) = default;
    virtual ~board() = default;
    uint8_t get(char file, int rank);
    uint8_t get(const std::string& position);
    void set(char file, int rank, uint8_t value);
    void set(const std::string& position, uint8_t value);
    int count();
    uint8_t& operator[](int index);
    const uint8_t& operator[](int index) const;
    static board create_start_board();
private:
    void set(int row, int column, uint8_t value);
    uint8_t field_[64];
};
