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
        queen_ = make_shared_board("8/8/8/8/3Q4/8/8/8 w - - 0 1");
        knight_ = make_shared_board("8/8/8/8/3N4/8/8/8 w - - 0 1");
        king_ = make_shared_board("8/8/8/5K2/8/8/8/8 w - - 0 1");
        rook_ = make_shared_board("8/8/8/3R4/8/8/8/8 w - - 0 1");
        bishop_ = make_shared_board("8/8/8/3B4/8/8/8/8 w - - 0 1");
    }

    virtual void TearDown() override {
    }

    std::shared_ptr<board> board_;
    std::shared_ptr<board> empty_;
    std::shared_ptr<board> queen_;
    std::shared_ptr<board> knight_;
    std::shared_ptr<board> king_;
    std::shared_ptr<board> rook_;
    std::shared_ptr<board> bishop_;
};

TEST_F(piece_test, pawn_single_and_double_movement) {
    auto p = std::make_shared<piece>(board_, "d2");
    p->update();
    auto m = p->allowed_moves();

    ASSERT_EQ(2, m.size());
    ASSERT_EQ("dd3", m[0]->to_string());
    ASSERT_EQ("dd4", m[1]->to_string());
}

TEST_F(piece_test, knight_startup) {
    auto p = std::make_shared<piece>(board_, "b1");
    p->update();
    auto m = p->allowed_moves();

    ASSERT_EQ(2, m.size());
    auto str1 = m[0]->to_string();
    auto str2 = m[1]->to_string();
    ASSERT_NE(str1, str2);
    ASSERT_TRUE(str1 == "Nba3" || str1 == "Nbc3");
    ASSERT_TRUE(str2 == "Nba3" || str2 == "Nbc3");
}

TEST_F(piece_test, queen_movement) {
    auto p = std::make_shared<piece>(queen_, "d4");
    p->update();
    auto m = p->allowed_moves();

    ASSERT_EQ(27, m.size());
    ASSERT_NE(std::end(m), std::find_if(std::begin(m), std::end(m), [] (std::shared_ptr<move> move) { return move->to_string() == "Qdd1"; }));
    ASSERT_NE(std::end(m), std::find_if(std::begin(m), std::end(m), [] (std::shared_ptr<move> move) { return move->to_string() == "Qdd8"; }));
    ASSERT_NE(std::end(m), std::find_if(std::begin(m), std::end(m), [] (std::shared_ptr<move> move) { return move->to_string() == "Qda4"; }));
    ASSERT_NE(std::end(m), std::find_if(std::begin(m), std::end(m), [] (std::shared_ptr<move> move) { return move->to_string() == "Qdh4"; }));
    ASSERT_NE(std::end(m), std::find_if(std::begin(m), std::end(m), [] (std::shared_ptr<move> move) { return move->to_string() == "Qda1"; }));
    ASSERT_NE(std::end(m), std::find_if(std::begin(m), std::end(m), [] (std::shared_ptr<move> move) { return move->to_string() == "Qda7"; }));
    ASSERT_NE(std::end(m), std::find_if(std::begin(m), std::end(m), [] (std::shared_ptr<move> move) { return move->to_string() == "Qdh8"; }));
    ASSERT_NE(std::end(m), std::find_if(std::begin(m), std::end(m), [] (std::shared_ptr<move> move) { return move->to_string() == "Qdg7"; }));
}

TEST_F(piece_test, knight_movement) {
    auto p = std::make_shared<piece>(knight_, "d4");
    p->update();
    auto m = p->allowed_moves();

    ASSERT_EQ(8, m.size());
    ASSERT_NE(std::end(m), std::find_if(std::begin(m), std::end(m), [] (std::shared_ptr<move> move) { return move->to_string() == "Nde6"; }));
    ASSERT_NE(std::end(m), std::find_if(std::begin(m), std::end(m), [] (std::shared_ptr<move> move) { return move->to_string() == "Ndf5"; }));
    ASSERT_NE(std::end(m), std::find_if(std::begin(m), std::end(m), [] (std::shared_ptr<move> move) { return move->to_string() == "Ndf3"; }));
    ASSERT_NE(std::end(m), std::find_if(std::begin(m), std::end(m), [] (std::shared_ptr<move> move) { return move->to_string() == "Nde2"; }));
    ASSERT_NE(std::end(m), std::find_if(std::begin(m), std::end(m), [] (std::shared_ptr<move> move) { return move->to_string() == "Ndc2"; }));
    ASSERT_NE(std::end(m), std::find_if(std::begin(m), std::end(m), [] (std::shared_ptr<move> move) { return move->to_string() == "Ndb3"; }));
    ASSERT_NE(std::end(m), std::find_if(std::begin(m), std::end(m), [] (std::shared_ptr<move> move) { return move->to_string() == "Ndb5"; }));
    ASSERT_NE(std::end(m), std::find_if(std::begin(m), std::end(m), [] (std::shared_ptr<move> move) { return move->to_string() == "Ndc6"; }));
}

TEST_F(piece_test, king_movement) {
    auto p = std::make_shared<piece>(king_, "f5");
    p->update();
    auto m = p->allowed_moves();

    ASSERT_EQ(8, m.size());
    ASSERT_NE(std::end(m), std::find_if(std::begin(m), std::end(m), [] (std::shared_ptr<move> move) { return move->to_string() == "Kff6"; }));
    ASSERT_NE(std::end(m), std::find_if(std::begin(m), std::end(m), [] (std::shared_ptr<move> move) { return move->to_string() == "Kfg6"; }));
    ASSERT_NE(std::end(m), std::find_if(std::begin(m), std::end(m), [] (std::shared_ptr<move> move) { return move->to_string() == "Kfg5"; }));
    ASSERT_NE(std::end(m), std::find_if(std::begin(m), std::end(m), [] (std::shared_ptr<move> move) { return move->to_string() == "Kfg4"; }));
    ASSERT_NE(std::end(m), std::find_if(std::begin(m), std::end(m), [] (std::shared_ptr<move> move) { return move->to_string() == "Kff4"; }));
    ASSERT_NE(std::end(m), std::find_if(std::begin(m), std::end(m), [] (std::shared_ptr<move> move) { return move->to_string() == "Kfe4"; }));
    ASSERT_NE(std::end(m), std::find_if(std::begin(m), std::end(m), [] (std::shared_ptr<move> move) { return move->to_string() == "Kfe5"; }));
    ASSERT_NE(std::end(m), std::find_if(std::begin(m), std::end(m), [] (std::shared_ptr<move> move) { return move->to_string() == "Kfe6"; }));
}

TEST_F(piece_test, rook_movement) {
    auto p = std::make_shared<piece>(rook_, "d5");
    p->update();
    auto m = p->allowed_moves();

    ASSERT_EQ(14, m.size());
    ASSERT_NE(std::end(m), std::find_if(std::begin(m), std::end(m), [] (std::shared_ptr<move> move) { return move->to_string() == "Rdd8"; }));
    ASSERT_NE(std::end(m), std::find_if(std::begin(m), std::end(m), [] (std::shared_ptr<move> move) { return move->to_string() == "Rdd1"; }));
    ASSERT_NE(std::end(m), std::find_if(std::begin(m), std::end(m), [] (std::shared_ptr<move> move) { return move->to_string() == "Rda5"; }));
    ASSERT_NE(std::end(m), std::find_if(std::begin(m), std::end(m), [] (std::shared_ptr<move> move) { return move->to_string() == "Rdg5"; }));
}

TEST_F(piece_test, bishop_movement) {
    auto p = std::make_shared<piece>(bishop_, "d5");
    p->update();
    auto m = p->allowed_moves();

    ASSERT_EQ(13, m.size());
    ASSERT_NE(std::end(m), std::find_if(std::begin(m), std::end(m), [] (std::shared_ptr<move> move) { return move->to_string() == "Bda8"; }));
    ASSERT_NE(std::end(m), std::find_if(std::begin(m), std::end(m), [] (std::shared_ptr<move> move) { return move->to_string() == "Bdg8"; }));
    ASSERT_NE(std::end(m), std::find_if(std::begin(m), std::end(m), [] (std::shared_ptr<move> move) { return move->to_string() == "Bdh1"; }));
    ASSERT_NE(std::end(m), std::find_if(std::begin(m), std::end(m), [] (std::shared_ptr<move> move) { return move->to_string() == "Bda2"; }));
}
