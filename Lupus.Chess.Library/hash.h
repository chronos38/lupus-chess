#pragma once

#include "board.h"

void zobrist_initialize();
uint64_t zobrist_hash(const board& board);
