#include <gtest/gtest.h>
#include <chess/board.h>
#include <chess/piece.h>

class board_test : public ::testing::Test
{
public:
    virtual void SetUp() override {
        empty_ = board();
        board_ = board::create_starting_board();
    }

    virtual void TearDown() override {
    }

    board board_;
    board empty_;
};

TEST_F(board_test, constructor)
{
    for (auto&& value : empty_) {
        ASSERT_EQ(0, value);
    }
}

TEST_F(board_test, starting_board)
{
    ASSERT_EQ(white_rook, board_.get("a1"));
    ASSERT_EQ(white_knight, board_.get("b1"));
    ASSERT_EQ(white_bishop, board_.get("c1"));
    ASSERT_EQ(white_queen, board_.get("d1"));
    ASSERT_EQ(white_king, board_.get("e1"));
    ASSERT_EQ(white_bishop, board_.get("f1"));
    ASSERT_EQ(white_knight, board_.get("g1"));
    ASSERT_EQ(white_rook, board_.get("h1"));
    for (auto i = 'a'; i != 'i'; i++)
        ASSERT_EQ(white_pawn, board_.get(i, 2));

    ASSERT_EQ(black_rook, board_.get("a8"));
    ASSERT_EQ(black_knight, board_.get("b8"));
    ASSERT_EQ(black_bishop, board_.get("c8"));
    ASSERT_EQ(black_queen, board_.get("d8"));
    ASSERT_EQ(black_king, board_.get("e8"));
    ASSERT_EQ(black_bishop, board_.get("f8"));
    ASSERT_EQ(black_knight, board_.get("g8"));
    ASSERT_EQ(black_rook, board_.get("h8"));
    for (auto i = 'a'; i != 'i'; i++)
        ASSERT_EQ(black_pawn, board_.get(i, 7));

    ASSERT_EQ(white, board_.active_color());
    ASSERT_EQ(0, strcmp("KQkq", board_.castling()));
    ASSERT_EQ(0, strcmp("-", board_.en_passant()));
    ASSERT_EQ(0, board_.halfmove());
    ASSERT_EQ(1, board_.fullmove());
}

TEST_F(board_test, to_fen) {
    auto board = make_board("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
    ASSERT_EQ("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1", board.to_fen());
    board = make_board("rnbqkbnr/pppppp2/2pp4/8/8/8/PPPPPPPP/RNBQKBNR b - - 40 3");
    ASSERT_EQ("rnbqkbnr/pppppp2/2pp4/8/8/8/PPPPPPPP/RNBQKBNR b - - 40 3", board.to_fen());
}

TEST_F(board_test, get_set_en_passant) {
    ASSERT_EQ(0, strcmp("-", board_.en_passant()));
    board_.set_en_passant("e3");
    ASSERT_EQ(0, strcmp("e3", board_.en_passant()));
    board_.set_en_passant("g3g3g3");
    ASSERT_EQ(0, strcmp("g3", board_.en_passant()));
}

TEST_F(board_test, get_set_castling) {
    ASSERT_EQ(0, strcmp("KQkq", board_.castling()));
    board_.set_castling("-");
    ASSERT_EQ(0, strcmp("-", board_.castling()));
    board_.set_castling("KQkqKQkq");
    ASSERT_EQ(0, strcmp("KQkq", board_.castling()));
}

TEST_F(board_test, count) {
    ASSERT_EQ(32, board_.count());
    ASSERT_EQ(0, empty_.count());
}

TEST_F(board_test, get_set) {
    empty_.set("a1", white_king);
    ASSERT_EQ(white_king, empty_.get("a1"));
    ASSERT_EQ(white_king, empty_.get('a', 1));
    empty_.set('a', 2, white_queen);
    ASSERT_EQ(white_queen, empty_.get("a2"));
    ASSERT_EQ(white_queen, empty_.get('a', 2));
}

TEST_F(board_test, toggle_active_color) {
    auto color = board_.active_color();
    board_.toggle_active_color();
    ASSERT_NE(color, board_.active_color());
}

TEST_F(board_test, get_set_halfmove_fullmove) {
    board_.set_fullmove(std::numeric_limits<uint8_t>::max());
    board_.set_halfmove(std::numeric_limits<uint8_t>::max());
    ASSERT_EQ(std::numeric_limits<uint8_t>::max(), board_.fullmove());
    ASSERT_EQ(std::numeric_limits<uint8_t>::max(), board_.halfmove());
}
