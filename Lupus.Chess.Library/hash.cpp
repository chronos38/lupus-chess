#include "hash.h"
#include <random>
#include <limits>
#include <unordered_map>

static uint64_t zobrist_table[768];
static std::unordered_map<uint8_t, int> piece_value_map = {
        { white_king, 0 },
        { white_queen, 1 },
        { white_rook, 2 },
        { white_bishop, 3 },
        { white_knight, 4 },
        { white_pawn, 5 },
        { black_king, 6 },
        { black_queen, 7 },
        { black_rook, 8 },
        { black_bishop, 9 },
        { black_knight, 10 },
        { black_pawn, 11 }
};

void zobrist_initialize() {
    std::random_device rd;
    std::mt19937 gen(rd());
    std::uniform_int_distribution<uint64_t> dis(
        std::numeric_limits<uint64_t>::min(),
        std::numeric_limits<uint64_t>::max());

    for (auto&& entry : zobrist_table)
        entry = dis(gen);
}


uint64_t zobrist_hash(const board& board) {
    uint64_t hash = 0;

    for (auto i = 0; i < 64; i++)
    {
        if (board[i])
        {
            auto j = piece_value_map[board[i]];
            hash = hash ^ zobrist_table[i * 12 + j];
        }
    }

    return hash;
}
