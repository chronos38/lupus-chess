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
        king_ = make_shared_board("PPPPPPPP/8/8/5K2/8/8/8/PPPPPPPP w - - 0 1");
        rook_ = make_shared_board("8/8/8/3R4/8/8/8/8 w - - 0 1");
        bishop_ = make_shared_board("8/8/8/3B4/8/8/8/8 w - - 0 1");
        pawn_ = make_shared_board("3p1p2/4P3/8/p1p5/1P6/4p1p1/5P2/8 w - - 0 1");
        black_pawn_ = make_shared_board("8/5p2/4P1P1/1p6/P1P5/8/4p3/3P1P2 b - - 0 1");
        black_king_ = make_shared_board("pppppppp/8/8/5k2/8/8/8/pppppppp b - - 0 1");
        end_king_ = make_shared_board("8/8/8/5K2/8/8/8/8 w - - 0 1");
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
    std::shared_ptr<board> pawn_;
    std::shared_ptr<board> black_pawn_;
    std::shared_ptr<board> black_king_;
    std::shared_ptr<board> end_king_;
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

TEST_F(piece_test, pawn_movements) {
    auto p1 = std::make_shared<piece>(pawn_, "e7");
    auto p2 = std::make_shared<piece>(pawn_, "b4");
    auto p3 = std::make_shared<piece>(pawn_, "f2");

    p1->update();
    p2->update();
    p3->update();

    auto m1 = p1->allowed_moves();
    auto m2 = p2->allowed_moves();
    auto m3 = p3->allowed_moves();

    ASSERT_EQ(3, m1.size());
    ASSERT_EQ(3, m2.size());
    ASSERT_EQ(4, m3.size());
    ASSERT_NE(std::end(m1), std::find_if(std::begin(m1), std::end(m1), [] (std::shared_ptr<move> move) { return move->to_string() == "ee8"; }));
    ASSERT_NE(std::end(m1), std::find_if(std::begin(m1), std::end(m1), [] (std::shared_ptr<move> move) { return move->to_string() == "ed8"; }));
    ASSERT_NE(std::end(m1), std::find_if(std::begin(m1), std::end(m1), [] (std::shared_ptr<move> move) { return move->to_string() == "ef8"; }));
    ASSERT_NE(std::end(m2), std::find_if(std::begin(m2), std::end(m2), [] (std::shared_ptr<move> move) { return move->to_string() == "bb5"; }));
    ASSERT_NE(std::end(m2), std::find_if(std::begin(m2), std::end(m2), [] (std::shared_ptr<move> move) { return move->to_string() == "ba5"; }));
    ASSERT_NE(std::end(m2), std::find_if(std::begin(m2), std::end(m2), [] (std::shared_ptr<move> move) { return move->to_string() == "bc5"; }));
    ASSERT_NE(std::end(m3), std::find_if(std::begin(m3), std::end(m3), [] (std::shared_ptr<move> move) { return move->to_string() == "ff3"; }));
    ASSERT_NE(std::end(m3), std::find_if(std::begin(m3), std::end(m3), [] (std::shared_ptr<move> move) { return move->to_string() == "ff4"; }));
    ASSERT_NE(std::end(m3), std::find_if(std::begin(m3), std::end(m3), [] (std::shared_ptr<move> move) { return move->to_string() == "fe3"; }));
    ASSERT_NE(std::end(m3), std::find_if(std::begin(m3), std::end(m3), [] (std::shared_ptr<move> move) { return move->to_string() == "fg3"; }));
}

TEST_F(piece_test, black_pawn_movement) {
    auto p1 = std::make_shared<piece>(black_pawn_, "f7");
    auto p2 = std::make_shared<piece>(black_pawn_, "b5");
    auto p3 = std::make_shared<piece>(black_pawn_, "e2");

    p1->update();
    p2->update();
    p3->update();

    auto m1 = p1->allowed_moves();
    auto m2 = p2->allowed_moves();
    auto m3 = p3->allowed_moves();

    ASSERT_EQ(4, m1.size());
    ASSERT_EQ(3, m2.size());
    ASSERT_EQ(3, m3.size());
    ASSERT_NE(std::end(m1), std::find_if(std::begin(m1), std::end(m1), [] (std::shared_ptr<move> move) { return move->to_string() == "ff6"; }));
    ASSERT_NE(std::end(m1), std::find_if(std::begin(m1), std::end(m1), [] (std::shared_ptr<move> move) { return move->to_string() == "ff5"; }));
    ASSERT_NE(std::end(m1), std::find_if(std::begin(m1), std::end(m1), [] (std::shared_ptr<move> move) { return move->to_string() == "fe6"; }));
    ASSERT_NE(std::end(m1), std::find_if(std::begin(m1), std::end(m1), [] (std::shared_ptr<move> move) { return move->to_string() == "fg6"; }));
    ASSERT_NE(std::end(m2), std::find_if(std::begin(m2), std::end(m2), [] (std::shared_ptr<move> move) { return move->to_string() == "bb4"; }));
    ASSERT_NE(std::end(m2), std::find_if(std::begin(m2), std::end(m2), [] (std::shared_ptr<move> move) { return move->to_string() == "ba4"; }));
    ASSERT_NE(std::end(m2), std::find_if(std::begin(m2), std::end(m2), [] (std::shared_ptr<move> move) { return move->to_string() == "bc4"; }));
    ASSERT_NE(std::end(m3), std::find_if(std::begin(m3), std::end(m3), [] (std::shared_ptr<move> move) { return move->to_string() == "ee1"; }));
    ASSERT_NE(std::end(m3), std::find_if(std::begin(m3), std::end(m3), [] (std::shared_ptr<move> move) { return move->to_string() == "ed1"; }));
    ASSERT_NE(std::end(m3), std::find_if(std::begin(m3), std::end(m3), [] (std::shared_ptr<move> move) { return move->to_string() == "ef1"; }));
}

TEST_F(piece_test, black_king_movement) {
    auto p = std::make_shared<piece>(black_king_, "f5");
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

TEST_F(piece_test, end_king_movement) {
    auto p = std::make_shared<piece>(end_king_, "f5");
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

TEST_F(piece_test, score) {
    auto p = std::make_shared<piece>(board_, "a2");
    auto r = std::make_shared<piece>(board_, "a1");
    auto n = std::make_shared<piece>(board_, "b1");
    auto b = std::make_shared<piece>(board_, "c1");
    auto q = std::make_shared<piece>(board_, "d1");
    auto k = std::make_shared<piece>(board_, "e1");

    ASSERT_EQ(100, p->score());
    ASSERT_EQ(525, r->score());
    ASSERT_EQ(350, n->score());
    ASSERT_EQ(350, b->score());
    ASSERT_EQ(1000, q->score());
    ASSERT_EQ(20000, k->score());
}

TEST_F(piece_test, attack_score) {
    auto p = std::make_shared<piece>(board_, "a2");
    auto r = std::make_shared<piece>(board_, "a1");
    auto n = std::make_shared<piece>(board_, "b1");
    auto b = std::make_shared<piece>(board_, "c1");
    auto q = std::make_shared<piece>(board_, "d1");
    auto k = std::make_shared<piece>(board_, "e1");

    ASSERT_EQ(0, p->attack_score());
    ASSERT_EQ(0, r->attack_score());
    ASSERT_EQ(0, n->attack_score());
    ASSERT_EQ(0, b->attack_score());
    ASSERT_EQ(0, q->attack_score());
    ASSERT_EQ(0, k->attack_score());
}

TEST_F(piece_test, defense_score) {
    auto p = std::make_shared<piece>(board_, "a2");
    auto r = std::make_shared<piece>(board_, "a1");
    auto n = std::make_shared<piece>(board_, "b1");
    auto b = std::make_shared<piece>(board_, "c1");
    auto q = std::make_shared<piece>(board_, "d1");
    auto k = std::make_shared<piece>(board_, "e1");

    ASSERT_EQ(0, p->defense_score());
    ASSERT_EQ(0, r->defense_score());
    ASSERT_EQ(0, n->defense_score());
    ASSERT_EQ(0, b->defense_score());
    ASSERT_EQ(0, q->defense_score());
    ASSERT_EQ(0, k->defense_score());
}

TEST_F(piece_test, position_score) {
    auto p = std::make_shared<piece>(board_, "a2");
    auto r = std::make_shared<piece>(board_, "a1");
    auto n = std::make_shared<piece>(board_, "b1");
    auto b = std::make_shared<piece>(board_, "c1");
    auto q = std::make_shared<piece>(board_, "d1");
    auto k = std::make_shared<piece>(board_, "e1");

    ASSERT_EQ(0, p->position_score());
    ASSERT_EQ(0, r->position_score());
    ASSERT_EQ(0, n->position_score());
    ASSERT_EQ(0, b->position_score());
    ASSERT_EQ(0, q->position_score());
    ASSERT_EQ(0, k->position_score());
}

TEST_F(piece_test, value) {
    auto p = std::make_shared<piece>(board_, "a2");
    auto r = std::make_shared<piece>(board_, "a1");
    auto n = std::make_shared<piece>(board_, "b1");
    auto b = std::make_shared<piece>(board_, "c1");
    auto q = std::make_shared<piece>(board_, "d1");
    auto k = std::make_shared<piece>(board_, "e1");

    ASSERT_EQ('P', p->value());
    ASSERT_EQ('R', r->value());
    ASSERT_EQ('N', n->value());
    ASSERT_EQ('B', b->value());
    ASSERT_EQ('Q', q->value());
    ASSERT_EQ('K', k->value());
}

TEST_F(piece_test, type) {
    auto p = std::make_shared<piece>(board_, "a2");
    auto r = std::make_shared<piece>(board_, "a1");
    auto n = std::make_shared<piece>(board_, "b1");
    auto b = std::make_shared<piece>(board_, "c1");
    auto q = std::make_shared<piece>(board_, "d1");
    auto k = std::make_shared<piece>(board_, "e1");

    ASSERT_EQ(pawn, p->type());
    ASSERT_EQ(rook, r->type());
    ASSERT_EQ(knight, n->type());
    ASSERT_EQ(bishop, b->type());
    ASSERT_EQ(queen, q->type());
    ASSERT_EQ(king, k->type());
}

TEST_F(piece_test, color) {
    auto p = std::make_shared<piece>(board_, "a2");
    auto r = std::make_shared<piece>(board_, "a1");
    auto n = std::make_shared<piece>(board_, "b1");
    auto b = std::make_shared<piece>(board_, "c1");
    auto q = std::make_shared<piece>(board_, "d1");
    auto k = std::make_shared<piece>(board_, "e1");

    ASSERT_EQ(white, p->color());
    ASSERT_EQ(white, r->color());
    ASSERT_EQ(white, n->color());
    ASSERT_EQ(white, b->color());
    ASSERT_EQ(white, q->color());
    ASSERT_EQ(white, k->color());
}

TEST_F(piece_test, position) {
    auto p = std::make_shared<piece>(board_, "a2");
    auto r = std::make_shared<piece>(board_, "a1");
    auto n = std::make_shared<piece>(board_, "b1");
    auto b = std::make_shared<piece>(board_, "c1");
    auto q = std::make_shared<piece>(board_, "d1");
    auto k = std::make_shared<piece>(board_, "e1");

    ASSERT_EQ(0, strcmp(p->position(), "a2"));
    ASSERT_EQ(0, strcmp(r->position(), "a1"));
    ASSERT_EQ(0, strcmp(n->position(), "b1"));
    ASSERT_EQ(0, strcmp(b->position(), "c1"));
    ASSERT_EQ(0, strcmp(q->position(), "d1"));
    ASSERT_EQ(0, strcmp(k->position(), "e1"));
}
