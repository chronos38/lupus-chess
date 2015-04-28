#pragma once
#include <memory>

namespace chess {
    class executor;
    int quiescence_search(std::shared_ptr<executor> exe, int alpha, int beta, int depth = 8);
    int alpha_beta_search(std::shared_ptr<executor> exe, int alpha, int beta, int depth = 8, int quiescene_depth = 8);
}
