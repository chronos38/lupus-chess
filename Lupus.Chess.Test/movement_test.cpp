#include <gtest/gtest.h>
#include <chess/movement.h>

using namespace chess;

class movement_test : public ::testing::Test {
public:
    virtual void SetUp() override {
    }

    virtual void TearDown() override {
    }
};

TEST_F(movement_test, mirror) {
    ASSERT_EQ("a1", mirror_position("h8"));
    ASSERT_EQ("a8", mirror_position("h1"));
    ASSERT_EQ("h1", mirror_position("a8"));
    ASSERT_EQ("h8", mirror_position("a1"));
    ASSERT_EQ("d4", mirror_position("e5"));
    ASSERT_EQ("d5", mirror_position("e4"));
    ASSERT_EQ("e4", mirror_position("d5"));
    ASSERT_EQ("e5", mirror_position("d4"));
    ASSERT_EQ("c1", mirror_position("f8"));
    ASSERT_EQ("f4", mirror_position("c5"));
}

TEST_F(movement_test, moves) {
    auto position = "d4";
    ASSERT_EQ("c4", move_left(position));
    ASSERT_EQ("e4", move_right(position));
    ASSERT_EQ("d5", move_up(position));
    ASSERT_EQ("d3", move_down(position));
    ASSERT_EQ("c5", move_upper_left(position));
    ASSERT_EQ("e5", move_upper_right(position));
    ASSERT_EQ("c3", move_lower_left(position));
    ASSERT_EQ("e3", move_lower_right(position));
}

TEST_F(movement_test, directions) {
    uint8_t collision = 0;
    auto board = board::create_shared_starting_board();
    auto result = move_direction(board, up, "a2", collision);
    ASSERT_EQ("a3", result);
    ASSERT_EQ(0, collision);
}

TEST_F(movement_test, till_end) {
    uint8_t collision = 0;
    auto board = board::create_shared_starting_board();
    auto result = moves_till_end(board, up, "a2", collision);
    ASSERT_EQ("a3", result.front());
    ASSERT_EQ("a7", result.back());
    ASSERT_EQ(black_pawn, collision);
}

TEST_F(movement_test, knight) {
    uint8_t collision = 0;
    auto board = board::create_shared_starting_board();
    auto result = move_knight(board, upper_right, up, "b1", collision);
    ASSERT_EQ("c3", result);
    ASSERT_EQ(0, collision);
}
