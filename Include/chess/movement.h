#pragma once
#include <string>
#include <cinttypes>
#include <vector>
#include "board.h"

namespace chess {
    enum direction {
        down,
        right,
        left,
        up,
        upper_left,
        upper_right,
        lower_left,
        lower_right
    };

    std::string mirror_position(const char* position);
    std::string move_left(const char* position);
    std::string move_right(const char* position);
    std::string move_up(const char* position);
    std::string move_down(const char* position);
    std::string move_upper_left(const char* position);
    std::string move_upper_right(const char* position);
    std::string move_lower_left(const char* position);
    std::string move_lower_right(const char* position);
    std::string move_direction(std::shared_ptr<board> board, direction dir, const char* position, uint8_t& collision);
    std::vector<std::string> moves_till_end(std::shared_ptr<board> board, direction dir, const char* position, uint8_t& collision);
    std::string move_knight(std::shared_ptr<board> board, direction first, direction second, const char* position, uint8_t& collision);
}
