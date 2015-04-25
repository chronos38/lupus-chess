#pragma once
#include <gmock/gmock.h>
#include <chess/piece.h>

class mock_piece : public ipiece {
public:
    MOCK_CONST_METHOD0(score, int());
    MOCK_CONST_METHOD0(attack_score, int());
    MOCK_CONST_METHOD0(defense_score, int());
    MOCK_CONST_METHOD0(position_score, int());
    MOCK_CONST_METHOD0(allowed_moves, std::vector<std::shared_ptr<move>>());
    MOCK_CONST_METHOD0(value, piece_value());
    MOCK_CONST_METHOD0(type, piece_type());
    MOCK_CONST_METHOD0(color, piece_color());
    MOCK_CONST_METHOD0(position, const char*());
    MOCK_CONST_METHOD0(board, std::shared_ptr<::board>());
};
