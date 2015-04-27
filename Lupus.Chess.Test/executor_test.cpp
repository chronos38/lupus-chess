#include <gtest/gtest.h>
#include <chess/board.h>
#include <chess/executor.h>
#include <chess/move.h>

using namespace chess;

class executor_test : public ::testing::Test {
public:
    virtual void SetUp() override {
        board_ = board::create_shared_starting_board();
    }

    virtual void TearDown() override {
    }

    std::shared_ptr<board> board_;
};

TEST_F(executor_test, allowed_start_moves_white) {
    // Arrange
    auto knight = 0;
    auto pawn = 0;
    auto exe = executor(board_);

    // Act
    exe.update();
    auto moves = exe.allowed_moves();
    for (auto&& move : moves) {
        if (move->to_string()[0] == 'N')
            knight++;
        else
            pawn++;
    }

    // Assert
    ASSERT_EQ(20, moves.size());
    ASSERT_EQ(4, knight);
    ASSERT_EQ(16, pawn);
}

TEST_F(executor_test, make_move) {
    // Arrange
    std::shared_ptr<move> knight;
    auto exe = executor(board_);

    // Act
    exe.update();
    for (auto&& move : exe.allowed_moves()) {
        if (strcmp(move->from(), "b1") == 0 && strcmp(move->to(), "c3") == 0) {
            knight = move;
            break;
        }
    }
    ASSERT_NE(nullptr, knight.get());
    exe.make_move(knight);

    // Assert
    ASSERT_EQ("rnbqkbnr/pppppppp/8/8/8/2N5/PPPPPPPP/R1BQKBNR b KQkq - 1 1", board_->to_fen());
}

TEST_F(executor_test, make_two_move) {
    // Arrange
    std::shared_ptr<move> knight;
    auto exe = executor(board_);

    // Act
    for (auto i = 0; i < 2; i++) {
        exe.update();
        for (auto&& move : exe.allowed_moves()) {
            if (move->from()[0] == 'b' && move->to()[0] == 'c') {
                knight = move;
                break;
            }
        }
        ASSERT_NE(nullptr, knight.get());
        exe.make_move(knight);
    }

    // Assert
    ASSERT_EQ("r1bqkbnr/pppppppp/2n5/8/8/2N5/PPPPPPPP/R1BQKBNR w KQkq - 2 2", board_->to_fen());
}

TEST_F(executor_test, undo_move) {
    // Arrange
    std::shared_ptr<move> knight;
    auto exe = executor(board_);

    // Act
    exe.update();
    for (auto&& move : exe.allowed_moves()) {
        if (strcmp(move->from(), "b1") == 0 && strcmp(move->to(), "c3") == 0) {
            knight = move;
            break;
        }
    }
    ASSERT_NE(nullptr, knight.get());
    exe.make_move(knight);
    exe.undo_move();

    // Assert
    ASSERT_EQ("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1", board_->to_fen());
}

TEST_F(executor_test, undo_two_move) {
    // Arrange
    std::shared_ptr<move> knight;
    auto exe = executor(board_);

    // Act
    for (auto i = 0; i < 2; i++) {
        exe.update();
        for (auto&& move : exe.allowed_moves()) {
            if (move->from()[0] == 'b' && move->to()[0] == 'c') {
                knight = move;
                break;
            }
        }
        ASSERT_NE(nullptr, knight.get());
        exe.make_move(knight);
    }

    exe.undo_move();
    exe.undo_move();

    // Assert
    ASSERT_EQ("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1", board_->to_fen());
}

TEST_F(executor_test, evaluate_without_update) {
    // Arrange
    auto exe = executor(board_);

    // Act
    auto eval = exe.evaluate();

    // Assert
    ASSERT_EQ(0, eval);
}

TEST_F(executor_test, evaluate_with_update) {
    // Arrange
    auto exe = executor(board_);

    // Act
    exe.update();
    auto eval = exe.evaluate();

    // Assert
    ASSERT_EQ(0, eval);
}

TEST_F(executor_test, clone) {
    // Arrange
    auto exe = executor(board_);
    auto clone = exe.clone();
}

TEST_F(executor_test, constructor) {
    auto exe = executor(board_);
    auto copy = executor(exe);
    auto move = std::move(executor(std::move(exe)));
}

TEST_F(executor_test, make_capture) {
    // Arrange
    auto board = make_shared_board("r7/1B6/8/8/8/8/8/8 w KQkq - 0 1");
    auto exe = executor(board);

    // Act
    exe.update();
    for (auto&& move : exe.allowed_moves()) {
        if (move->to()[0] == 'a' && move->to()[1] == '8') {
            exe.make_move(move);
            break;
        }
    }

    // Assert
    ASSERT_EQ("B7/8/8/8/8/8/8/8 b KQkq - 1 1", board->to_fen());
}

TEST_F(executor_test, undo_capture) {
    // Arrange
    auto board = make_shared_board("r7/1B6/8/8/8/8/8/8 w KQkq - 0 1");
    auto exe = executor(board);

    // Act
    exe.update();
    for (auto&& move : exe.allowed_moves()) {
        if (move->to()[0] == 'a' && move->to()[1] == '8') {
            exe.make_move(move);
            break;
        }
    }
    exe.undo_move();

    // Assert
    ASSERT_EQ("r7/1B6/8/8/8/8/8/8 w KQkq - 0 1", board->to_fen());
}
