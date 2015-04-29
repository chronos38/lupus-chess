#include <gtest/gtest.h>
#include <chess/executor.h>
#include <chess/search.h>
#include <chess/board.h>
#include <chess/move.h>
#include <chrono>

using namespace chess;

class search_test : public ::testing::Test {
public:
    virtual void SetUp() override {
        board_ = make_shared_board("rnbqkbnr/pppppppp/8/8/8/2N5/PPPPPPPP/R1BQKBNR b KQkq - 1 1");
        exe_ = std::make_shared<executor>(board_);
    }

    virtual void TearDown() override {
    }

    std::shared_ptr<board> board_;
    std::shared_ptr<executor> exe_;
};

TEST_F(search_test, is_0) {
    auto start = std::chrono::system_clock::now();
    auto move = iterative_search(exe_, 0);
    auto end = std::chrono::system_clock::now();
    auto elapsed = end - start;
    auto ms = std::chrono::duration_cast<std::chrono::milliseconds>(elapsed).count();
    std::cout << ms << " ms " << move->to_string() << std::endl;
}

TEST_F(search_test, is_1) {
    auto start = std::chrono::system_clock::now();
    auto move = iterative_search(exe_, 1);
    auto end = std::chrono::system_clock::now();
    auto elapsed = end - start;
    auto ms = std::chrono::duration_cast<std::chrono::milliseconds>(elapsed).count();
    std::cout << ms << " ms " << move->to_string() << std::endl;
}

TEST_F(search_test, is_2) {
    auto start = std::chrono::system_clock::now();
    auto move = iterative_search(exe_, 2);
    auto end = std::chrono::system_clock::now();
    auto elapsed = end - start;
    auto ms = std::chrono::duration_cast<std::chrono::milliseconds>(elapsed).count();
    std::cout << ms << " ms " << move->to_string() << std::endl;
}

TEST_F(search_test, is_3) {
    auto start = std::chrono::system_clock::now();
    auto move = iterative_search(exe_, 3);
    auto end = std::chrono::system_clock::now();
    auto elapsed = end - start;
    auto ms = std::chrono::duration_cast<std::chrono::milliseconds>(elapsed).count();
    std::cout << ms << " ms " << move->to_string() << std::endl;
}

TEST_F(search_test, is_4) {
    auto start = std::chrono::system_clock::now();
    auto move = iterative_search(exe_, 4);
    auto end = std::chrono::system_clock::now();
    auto elapsed = end - start;
    auto ms = std::chrono::duration_cast<std::chrono::milliseconds>(elapsed).count();
    std::cout << ms << " ms " << move->to_string() << std::endl;
}
