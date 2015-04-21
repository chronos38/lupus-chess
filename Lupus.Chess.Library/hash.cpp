#include "hash.h"
#include <random>
#include <limits>

static uint64_t zobrist_table[768];

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
            auto j = board[i] - 1;
            hash = hash ^ zobrist_table[i * 12 + j];
        }
    }

    return hash;
}
