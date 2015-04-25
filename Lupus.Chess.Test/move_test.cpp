#include <gtest/gtest.h>
#include <chess/move.h>
#include <chess/piece.h>
#include <chess/board.h>
#include "mock_piece.h"

class move_test : public ::testing::Test {
public:
    virtual void SetUp() override {
        board_ = make_shared_board("8/8/8/8/8/8/8/Q7 w - - 0 1");
        mock_ = new mock_piece();
        piece_ = mock_;

        ON_CALL(*mock_, score()).
            WillByDefault(testing::Return(10000));
        ON_CALL(*mock_, attack_score()).
            WillByDefault(testing::Return(0));
        ON_CALL(*mock_, defense_score()).
            WillByDefault(testing::Return(0));
        ON_CALL(*mock_, position_score()).
            WillByDefault(testing::Return(0));
        ON_CALL(*mock_, allowed_moves()).
            WillByDefault(testing::Return(std::vector<std::shared_ptr<move>>()));
        ON_CALL(*mock_, value()).
            WillByDefault(testing::Return(white_queen));
        ON_CALL(*mock_, type()).
            WillByDefault(testing::Return(queen));
        ON_CALL(*mock_, color()).
            WillByDefault(testing::Return(white));
        ON_CALL(*mock_, position()).
            WillByDefault(testing::Return("a1"));
        ON_CALL(*mock_, board()).
            WillByDefault(testing::Return(board_));
    }

    virtual void TearDown() override {
        delete mock_;
    }

    std::shared_ptr<board> board_;
    mock_piece* mock_;
    ipiece* piece_ = nullptr;
};

TEST_F(move_test, constructor) {
    EXPECT_NO_THROW({
        move(piece_, "a1", "h8");
        move(board_, queen_side, white);
    });

    EXPECT_THROW({
        move(board_, no_side, white);
    }, std::invalid_argument);

    EXPECT_THROW({
        move(board_, both_sides, white);
    }, std::invalid_argument);

    EXPECT_THROW({
        move(piece_, "", "");
    }, std::invalid_argument);

    EXPECT_THROW({
        move(piece_, "1a", "8h");
    }, std::invalid_argument);
}

TEST_F(move_test, execute_valid_move) {
    // Arrange
    auto m = std::make_shared<move>(piece_, "a1", "h8");

    // Act
    m->execute();

    // Assert
    ASSERT_EQ(0, board_->get("a1"));
    ASSERT_EQ('Q', board_->get("h8"));
}

TEST_F(move_test, undo_move) {
    // Arrange
    auto m = std::make_shared<move>(piece_, "a1", "h8");

    // Act
    m->execute();
    m->undo();

    // Assert
    ASSERT_EQ('Q', board_->get("a1"));
    ASSERT_EQ(0, board_->get("h8"));
}

TEST_F(move_test, to_string_piece) {
    // Arrange
    auto m = std::make_shared<move>(piece_, "a1", "h8");

    // Act
    auto string = m->to_string();

    // Assert
    ASSERT_EQ("Qah8", string);
}

TEST_F(move_test, to_string_castling) {
    // Arrange
    auto m1 = std::make_shared<move>(board_, king_side, white);
    auto m2 = std::make_shared<move>(board_, queen_side, white);

    // Act
    auto s1 = m1->to_string();
    auto s2 = m2->to_string();

    // Assert
    ASSERT_EQ("O-O", s1);
    ASSERT_EQ("O-O-O", s2);
}

TEST_F(move_test, equal) {
    // Arrange
    auto m1 = std::make_shared<move>(piece_, "a1", "h8");
    auto m2 = std::make_shared<move>(piece_, "a1", "h8");
    auto m3 = std::make_shared<move>(piece_, "a1", "a8");
    auto m4 = std::make_shared<move>(piece_, "a1", "h1");
    auto m5 = std::make_shared<move>(piece_, "b2", "c2");
    auto c1 = std::make_shared<move>(board_, king_side, white);
    auto c2 = std::make_shared<move>(board_, king_side, white);
    auto c3 = std::make_shared<move>(board_, queen_side, black);

    // Act
    auto b1 = *m1 == *m2;
    auto b2 = *m1 == *m3;
    auto b3 = *m1 == *m4;
    auto b4 = *m1 == *m5;
    auto b5 = *c1 == *c2;
    auto b6 = *c1 == *c3;
    auto b7 = *c2 == *c3;

    // Assert
    ASSERT_TRUE(b1);
    ASSERT_FALSE(b2);
    ASSERT_FALSE(b3);
    ASSERT_FALSE(b4);
    ASSERT_TRUE(b5);
    ASSERT_FALSE(b6);
    ASSERT_FALSE(b7);
}
