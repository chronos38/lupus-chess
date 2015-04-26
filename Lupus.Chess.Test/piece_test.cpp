#include <gtest/gtest.h>
#include <chess/board.h>
#include <chess/piece.h>
#include <chess/move.h>

using namespace chess;

class piece_test : public ::testing::Test {
public:
    virtual void SetUp() override {
        empty_ = std::make_shared<board>();
        board_ = board::create_shared_starting_board();
    }

    virtual void TearDown() override {
    }

    std::shared_ptr<board> board_;
    std::shared_ptr<board> empty_;
};

TEST_F(piece_test, pawn_single_and_double_movement) {
    auto p = std::make_shared<piece>(board_, "d2");
    p->update();
    auto m = p->allowed_moves();

    ASSERT_EQ(2, m.size());
    ASSERT_EQ("dd3", m[0]->to_string());
    ASSERT_EQ("dd4", m[1]->to_string());
}
