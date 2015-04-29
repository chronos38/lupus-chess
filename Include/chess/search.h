#pragma once
#include <memory>

namespace chess {
    static const int initial_alpha = -std::numeric_limits<int>::max();
    static const int initial_beta = std::numeric_limits<int>::max();
    static const int quiescence_search_depth = 2;
    class executor;
    class move;
    int quiescence_search(std::shared_ptr<executor> exe, int alpha, int beta, int depth);
    int alpha_beta_search(std::shared_ptr<executor> exe, int alpha, int beta, int depth);
    std::shared_ptr<move> iterative_search(std::shared_ptr<executor> exe, int depth);
}
