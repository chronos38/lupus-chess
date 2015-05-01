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
    }

    virtual void TearDown() override {
    }

    std::shared_ptr<board> board_;
    std::shared_ptr<executor> exe_;
};

TEST_F(search_test, is_0_early) {
    board_ = make_shared_board("rnbqkbnr/pppppppp/8/8/8/2N5/PPPPPPPP/R1BQKBNR b KQkq - 1 1");
    exe_ = std::make_shared<executor>(board_);
    auto start = std::chrono::system_clock::now();
    auto move = iterative_search(exe_, 0);
    auto end = std::chrono::system_clock::now();
    auto elapsed = end - start;
    auto ms = std::chrono::duration_cast<std::chrono::milliseconds>(elapsed).count();
    std::cout << ms << " ms " << (move ? move->to_string() : "terminal") << std::endl;
}

TEST_F(search_test, is_1_early) {
    board_ = make_shared_board("rnbqkbnr/pppppppp/8/8/8/2N5/PPPPPPPP/R1BQKBNR b KQkq - 1 1");
    exe_ = std::make_shared<executor>(board_);
    auto start = std::chrono::system_clock::now();
    auto move = iterative_search(exe_, 1);
    auto end = std::chrono::system_clock::now();
    auto elapsed = end - start;
    auto ms = std::chrono::duration_cast<std::chrono::milliseconds>(elapsed).count();
    std::cout << ms << " ms " << (move ? move->to_string() : "terminal") << std::endl;
}

TEST_F(search_test, is_2_early) {
    board_ = make_shared_board("rnbqkbnr/pppppppp/8/8/8/2N5/PPPPPPPP/R1BQKBNR b KQkq - 1 1");
    exe_ = std::make_shared<executor>(board_);
    auto start = std::chrono::system_clock::now();
    auto move = iterative_search(exe_, 2);
    auto end = std::chrono::system_clock::now();
    auto elapsed = end - start;
    auto ms = std::chrono::duration_cast<std::chrono::milliseconds>(elapsed).count();
    std::cout << ms << " ms " << (move ? move->to_string() : "terminal") << std::endl;
}

TEST_F(search_test, is_0_middle) {
    board_ = make_shared_board("r3r1k1/ppbq1pp1/2p2n1p/2Pp1b2/1P1Pn3/3BBN1P/P1Q1NPP1/R3R1K1 b - - 1 1");
    exe_ = std::make_shared<executor>(board_);
    auto start = std::chrono::system_clock::now();
    auto move = iterative_search(exe_, 0);
    auto end = std::chrono::system_clock::now();
    auto elapsed = end - start;
    auto ms = std::chrono::duration_cast<std::chrono::milliseconds>(elapsed).count();
    std::cout << ms << " ms " << (move ? move->to_string() : "terminal") << std::endl;
}

TEST_F(search_test, is_1_middle) {
    board_ = make_shared_board("r3r1k1/ppbq1pp1/2p2n1p/2Pp1b2/1P1Pn3/3BBN1P/P1Q1NPP1/R3R1K1 b - - 1 1");
    exe_ = std::make_shared<executor>(board_);
    auto start = std::chrono::system_clock::now();
    auto move = iterative_search(exe_, 1);
    auto end = std::chrono::system_clock::now();
    auto elapsed = end - start;
    auto ms = std::chrono::duration_cast<std::chrono::milliseconds>(elapsed).count();
    std::cout << ms << " ms " << (move ? move->to_string() : "terminal") << std::endl;
}

TEST_F(search_test, is_2_middle) {
    board_ = make_shared_board("r3r1k1/ppbq1pp1/2p2n1p/2Pp1b2/1P1Pn3/3BBN1P/P1Q1NPP1/R3R1K1 b - - 1 1");
    exe_ = std::make_shared<executor>(board_);
    auto start = std::chrono::system_clock::now();
    auto move = iterative_search(exe_, 2);
    auto end = std::chrono::system_clock::now();
    auto elapsed = end - start;
    auto ms = std::chrono::duration_cast<std::chrono::milliseconds>(elapsed).count();
    std::cout << ms << " ms " << (move ? move->to_string() : "terminal") << std::endl;
}

TEST_F(search_test, is_0_end) {
    board_ = make_shared_board("7r/3p3p/1k6/1q4p1/1N2n1P1/PK2P3/6RP/3Q4 b - - 1 1");
    exe_ = std::make_shared<executor>(board_);
    auto start = std::chrono::system_clock::now();
    auto move = iterative_search(exe_, 0);
    auto end = std::chrono::system_clock::now();
    auto elapsed = end - start;
    auto ms = std::chrono::duration_cast<std::chrono::milliseconds>(elapsed).count();
    std::cout << ms << " ms " << (move ? move->to_string() : "terminal") << std::endl;
}

TEST_F(search_test, is_1_end) {
    board_ = make_shared_board("7r/3p3p/1k6/1q4p1/1N2n1P1/PK2P3/6RP/3Q4 b - - 1 1");
    exe_ = std::make_shared<executor>(board_);
    auto start = std::chrono::system_clock::now();
    auto move = iterative_search(exe_, 1);
    auto end = std::chrono::system_clock::now();
    auto elapsed = end - start;
    auto ms = std::chrono::duration_cast<std::chrono::milliseconds>(elapsed).count();
    std::cout << ms << " ms " << (move ? move->to_string() : "terminal") << std::endl;
}

TEST_F(search_test, is_2_end) {
    board_ = make_shared_board("7r/3p3p/1k6/1q4p1/1N2n1P1/PK2P3/6RP/3Q4 b - - 1 1");
    exe_ = std::make_shared<executor>(board_);
    auto start = std::chrono::system_clock::now();
    auto move = iterative_search(exe_, 2);
    auto end = std::chrono::system_clock::now();
    auto elapsed = end - start;
    auto ms = std::chrono::duration_cast<std::chrono::milliseconds>(elapsed).count();
    std::cout << ms << " ms " << (move ? move->to_string() : "terminal") << std::endl;
}
