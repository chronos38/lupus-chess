#pragma once
#include <memory>

namespace chess {
    static const int initial_alpha = -std::numeric_limits<int>::max() / 2;
    static const int initial_beta = std::numeric_limits<int>::max() / 2;
    static const int quiescence_search_depth = 4;
    static const int endgame_transition_count = 16;
    static const int queen_material_score = 1000;
    static const int king_material_score = 20000;

    class executor;
    class move;

    int quiescence_search_normal(std::shared_ptr<executor> exe, int alpha, int beta, int depth);
    int quiescence_search_endgame(std::shared_ptr<executor> exe, int alpha, int beta, int depth);
    int alpha_beta_search(std::shared_ptr<executor> exe, int alpha, int beta, int depth);
    std::shared_ptr<move> iterative_search(std::shared_ptr<executor> exe, int depth);
}
