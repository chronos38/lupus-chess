#pragma once
#include <cinttypes>
#include <memory>

class board;
void zobrist_initialize();
uint64_t zobrist_hash(const board& board);
uint64_t zobrist_hash(std::shared_ptr<board> board);
