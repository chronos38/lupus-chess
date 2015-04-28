#include <gtest/gtest.h>
#include <chess/executor.h>
#include <chess/search.h>
#include <chess/board.h>
#include <chrono>

using namespace chess;

class search_test : public ::testing::Test {
public:
    virtual void SetUp() override {
        board_ = board::create_shared_starting_board();
        exe_ = std::make_shared<executor>(board_);
    }

    virtual void TearDown() override {
    }

    std::shared_ptr<board> board_;
    std::shared_ptr<executor> exe_;
};

TEST_F(search_test, depth_1_to_5) {
    for (auto i = 1; i < 6; i++) {
        auto start = std::chrono::system_clock::now();
        auto score = alpha_beta_search(exe_, -std::numeric_limits<int>::max(), std::numeric_limits<int>::max(), i, i);
        auto end = std::chrono::system_clock::now();
        auto elapsed = end - start;
        std::cout << " depth: " << i << " needed "
            << std::chrono::duration_cast<std::chrono::milliseconds>(elapsed).count()
            << " ms" << " with score " << score << std::endl;
        board_ = board::create_shared_starting_board();
        exe_ = std::make_unique<executor>(board_);
    }
}
