#include <gtest/gtest.h>
#include <chess/move.h>
#include <chess/board.h>
#include "mock_piece.h"

class move_test : public ::testing::Test {
public:
    virtual void SetUp() override {
        piece_ = nullptr;
        left_rook_ = nullptr;
        right_rook_ = nullptr;
        king_ = nullptr;
    }

    virtual void TearDown() override {
        if (piece_) delete piece_;
        if (left_rook_) delete left_rook_;
        if (right_rook_) delete right_rook_;
        if (king_) delete king_;
    }

    std::shared_ptr<board> board_;
    mock_piece* piece_ = nullptr;
    mock_piece* left_rook_ = nullptr;
    mock_piece* right_rook_ = nullptr;
    mock_piece* king_ = nullptr;
};

void prepare_queen(move_test* test) {
    test->board_ = make_shared_board("8/8/8/8/8/8/8/Q7 w - - 0 1");
    test->piece_ = new mock_piece();

    ON_CALL(*test->piece_, score()).
        WillByDefault(testing::Return(1000));
    ON_CALL(*test->piece_, attack_score()).
        WillByDefault(testing::Return(0));
    ON_CALL(*test->piece_, defense_score()).
        WillByDefault(testing::Return(0));
    ON_CALL(*test->piece_, position_score()).
        WillByDefault(testing::Return(0));
    ON_CALL(*test->piece_, allowed_moves()).
        WillByDefault(testing::Return(std::vector<std::shared_ptr<move>>()));
    ON_CALL(*test->piece_, value()).
        WillByDefault(testing::Return(white_queen));
    ON_CALL(*test->piece_, type()).
        WillByDefault(testing::Return(queen));
    ON_CALL(*test->piece_, color()).
        WillByDefault(testing::Return(white));
    ON_CALL(*test->piece_, position()).
        WillByDefault(testing::Return("a1"));
    ON_CALL(*test->piece_, board()).
        WillByDefault(testing::Return(test->board_));
}

void prepare_castling(move_test* test) {
    test->board_ = make_shared_board("r3k2r/p3p2p/8/8/8/8/P3P2P/R3K2R w KQkq - 0 1");
    test->king_ = new mock_piece();
    test->left_rook_ = new mock_piece();
    test->right_rook_ = new mock_piece();

    ON_CALL(*test->king_, score()).
        WillByDefault(testing::Return(20000));
    ON_CALL(*test->king_, attack_score()).
        WillByDefault(testing::Return(0));
    ON_CALL(*test->king_, defense_score()).
        WillByDefault(testing::Return(0));
    ON_CALL(*test->king_, position_score()).
        WillByDefault(testing::Return(0));
    ON_CALL(*test->king_, allowed_moves()).
        WillByDefault(testing::Return(std::vector<std::shared_ptr<move>>()));
    ON_CALL(*test->king_, value()).
        WillByDefault(testing::Return(white_king));
    ON_CALL(*test->king_, type()).
        WillByDefault(testing::Return(king));
    ON_CALL(*test->king_, color()).
        WillByDefault(testing::Return(white));
    ON_CALL(*test->king_, position()).
        WillByDefault(testing::Return("e1"));
    ON_CALL(*test->king_, board()).
        WillByDefault(testing::Return(test->board_));

    for (auto i = 0; i < 2; i++) {
        auto rook = i ? test->left_rook_ : test->right_rook_;
        ON_CALL(*rook, score()).
            WillByDefault(testing::Return(525));
        ON_CALL(*rook, attack_score()).
            WillByDefault(testing::Return(0));
        ON_CALL(*rook, defense_score()).
            WillByDefault(testing::Return(0));
        ON_CALL(*rook, position_score()).
            WillByDefault(testing::Return(0));
        ON_CALL(*rook, allowed_moves()).
            WillByDefault(testing::Return(std::vector<std::shared_ptr<move>>()));
        ON_CALL(*rook, value()).
            WillByDefault(testing::Return(white_rook));
        ON_CALL(*rook, type()).
            WillByDefault(testing::Return(piece_type::rook));
        ON_CALL(*rook, color()).
            WillByDefault(testing::Return(white));
        ON_CALL(*rook, position()).
            WillByDefault(testing::Return(i ? "a1" : "h8"));
        ON_CALL(*rook, board()).
            WillByDefault(testing::Return(test->board_));
    }
}

TEST_F(move_test, constructor) {
    prepare_queen(this);

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
    prepare_queen(this);
    auto m = std::make_shared<move>(piece_, "a1", "h8");

    // Act
    m->execute();

    // Assert
    ASSERT_EQ(0, board_->get("a1"));
    ASSERT_EQ('Q', board_->get("h8"));
}

TEST_F(move_test, undo_move) {
    // Arrange
    prepare_queen(this);
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
    prepare_queen(this);
    auto m = std::make_shared<move>(piece_, "a1", "h8");

    // Act
    auto string = m->to_string();

    // Assert
    ASSERT_EQ("Qah8", string);
}

TEST_F(move_test, to_string_castling) {
    // Arrange
    prepare_queen(this);
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
    prepare_queen(this);
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

TEST_F(move_test, castling_queen_side_execute) {
    // Arrange
    prepare_castling(this);
    auto m = std::make_shared<move>(board_, queen_side, white);

    // Act
    m->execute();

    // Assert
    ASSERT_EQ(white_king, board_->get("c1"));
    ASSERT_EQ(white_rook, board_->get("d1"));
    ASSERT_EQ(0, board_->get("a1"));
    ASSERT_EQ(0, board_->get("e1"));
    ASSERT_EQ(0, strcmp(board_->castling(), "kq"));
}

TEST_F(move_test, castling_queen_side_undo) {
    // Arrange
    prepare_castling(this);
    auto m = std::make_shared<move>(board_, queen_side, white);

    // Act
    m->execute();
    m->undo();
    auto c = board_->castling();
    auto e = c + strlen(c);

    // Assert
    ASSERT_EQ(white_king, board_->get("e1"));
    ASSERT_EQ(white_rook, board_->get("a1"));
    ASSERT_EQ(0, board_->get("c1"));
    ASSERT_EQ(0, board_->get("d1"));
    ASSERT_NE(e, std::find(c, e, 'K'));
    ASSERT_NE(e, std::find(c, e, 'Q'));
    ASSERT_NE(e, std::find(c, e, 'k'));
    ASSERT_NE(e, std::find(c, e, 'q'));
}

TEST_F(move_test, castling_king_side_execute) {
    // Arrange
    prepare_castling(this);
    auto m = std::make_shared<move>(board_, king_side, white);

    // Act
    m->execute();

    // Assert
    ASSERT_EQ(white_king, board_->get("g1"));
    ASSERT_EQ(white_rook, board_->get("f1"));
    ASSERT_EQ(0, board_->get("h1"));
    ASSERT_EQ(0, board_->get("e1"));
    ASSERT_EQ(0, strcmp(board_->castling(), "kq"));
}

TEST_F(move_test, castling_king_side_undo) {
    // Arrange
    prepare_castling(this);
    auto m = std::make_shared<move>(board_, king_side, white);

    // Act
    m->execute();
    m->undo();
    auto c = board_->castling();
    auto e = c + strlen(c);

    // Assert
    ASSERT_EQ(white_king, board_->get("e1"));
    ASSERT_EQ(white_rook, board_->get("h1"));
    ASSERT_EQ(0, board_->get("g1"));
    ASSERT_EQ(0, board_->get("f1"));
    ASSERT_NE(e, std::find(c, e, 'K'));
    ASSERT_NE(e, std::find(c, e, 'Q'));
    ASSERT_NE(e, std::find(c, e, 'k'));
    ASSERT_NE(e, std::find(c, e, 'q'));
}
